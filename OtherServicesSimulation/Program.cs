using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace OtherServicesSimulation
{
    // This simulates the other parts of the system:
    // - Notification Service is simulated by a message consumer reacting on incoming messages of type SendOrderNotification 
    // - Product Service is simulated by emitting messages of type ProductRenamed
    class Program
    {
        private static Random rnd = new Random();

        // First, let's define some product id's and product names.
        // Each message of type ProductRenamed consists of a random tuple out of the following arrays
        private static string[] productIds = new[]
        {
            "P-123", "P-234", "P-345"
        };

        private static string[] productNames = new[]
        {
            "Sea Monkey",
            "Jolly Rasta",
            "Mad Monkey",
            "Screaming Narwhal",
            "Sea Cucumber",
            "Death Starfish"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Notification and Product Service Simulation");

            // Bus Initialization
            var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                config.ReceiveEndpoint(host, "notification-queue", ep => {
                    ep.Consumer<NotificationServiceConsumer>(); // The consumer is registered explicitly this time.
                });
            });
            busControl.Start();

            var cts = CreateProductUpdateEmitter(busControl);
            Console.ReadKey();
            cts.Cancel();
        }

        private static CancellationTokenSource CreateProductUpdateEmitter(IBusControl busControl)
        {
            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            Task.Factory.StartNew(() =>
            {
                // Emit one event for each product to initially "register" the products in the
                // order service. In a real system, the order service would listen for ProductCreated event messages.
                foreach (var id in productIds)
                {
                    busControl.Publish(new ProductRenamed(id, GetRandom(productNames)));
                }

                while (true)
                {
                    Thread.Sleep(5000); // Product Names change quite often ...
                    busControl.Publish(new ProductRenamed(GetRandom(productIds), GetRandom(productNames)));

                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }, ct);

            return cts;
        }

        private static T GetRandom<T>(T[] items) => items[rnd.Next(0, items.Length)];
    }

    public class NotificationServiceConsumer : IConsumer<SendOrderNotification>
    {
        public Task Consume(ConsumeContext<SendOrderNotification> context)
        {
            var msg = context.Message;
            Console.WriteLine($"Order with {msg.OrderId} accepted. Shipping to {msg.AddressLabel}");
            return Task.FromResult(0);
        }
    }
}