using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.DataObjects;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain;
using OrderService.Infrastructure;

namespace OrderService.Controllers
{
    [Route("api/orders")]
    public class OrderHistoryController
    {
        private readonly IOrderRepository orderRepository;

        private readonly IProductRepository productRepository;

        private readonly IBusControl busControl;

        public OrderHistoryController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IBusControl busControl)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.busControl = busControl;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto dto)
        {
            // In an asynchronous or command-based architecture like CQRS, you could employ mass transit
            // To dispatch a PlaceOrder command to a consumer processing the order.
            // For simplicty, we will not add another layer of complexity here.
            var address = new Address(dto.Address.Street, dto.Address.Zip, dto.Address.City, dto.Address.CountryIso);
            var orderLines = dto.OrderLines.Select(ol => new OrderLine(ol.ProductId, ol.Amount)).ToList();
            var order = new Order(address, orderLines);
            orderRepository.Insert(order, order.Id);

            // Sending message should be done in a more encapsulated and configurable way, of course.
            var endpoint = await busControl.GetSendEndpoint(new Uri("rabbitmq://localhost/notification-queue"));
            await endpoint.Send(new SendOrderNotification(order.Id, address.GetLabel(), DateTime.Now.AddDays(3)));

            return new OkResult();
        }

        [HttpGet]
        public IList<OrderDto> GetHistory()
        {
            // To keep it simple, all orders belong to one customer by definition.
            var orders = orderRepository.Query().ToList();
            var products = productRepository.Query();

            // Project the list of orders to a list of order DTOs.
            // If you want to avoid mapper code like this, take a look at
            // Automapper (automapper.org)
            var result = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderLines = o.OrderLines.Select(ol => new OrderLineDto
                {
                    Amount = ol.Amount,
                    ProductId = ol.ProductId,
                    ProductName = products.FirstOrDefault(p => p.ProductId == ol.ProductId)?.Name
                }).ToList(),
                Address = new AddressDto
                {
                    Street = o.Address.Street,
                    Zip = o.Address.Zip,
                    City = o.Address.City,
                    CountryIso = o.Address.CountryIso
                }
            });

            return result.ToList();
        }
    }
}
