using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MassTransit.Util;
using OrderService.Domain;
using OrderService.Infrastructure;

namespace OrderService.Consumers
{
    public class ProductRenamedConsumer : IConsumer<ProductRenamed>
    {
        private readonly IProductRepository productRepository;

        public ProductRenamedConsumer(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Task Consume(ConsumeContext<ProductRenamed> context)
        {
            var ev = context.Message;

            var product = productRepository.GetByKey(ev.ProductId);
            if (product == null)
            {
                Console.WriteLine($"Inserting new Product {ev.ProductId} with name {ev.Name}");
                productRepository.Insert(new Product(ev.ProductId, ev.Name), ev.ProductId);
            }
            else
            {
                Console.WriteLine($"Product {ev.ProductId} is now called {ev.Name}");
                product.Rename(ev.Name);
                productRepository.Update(product, ev.ProductId);
            }

            return TaskUtil.Completed;
        }
    }
}
