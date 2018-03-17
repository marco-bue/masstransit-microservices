using OrderService.Domain;

namespace OrderService.Infrastructure
{
    public interface IOrderRepository : IRepository<Order, string> { }

    public class OrderRepository : Repository<Order, string>, IOrderRepository
    {
    }
}
