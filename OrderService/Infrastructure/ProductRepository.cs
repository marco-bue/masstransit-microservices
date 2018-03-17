using OrderService.Domain;

namespace OrderService.Infrastructure
{
    public interface IProductRepository : IRepository<Product, string> { }

    public class ProductRepository : Repository<Product, string>, IProductRepository
    {
    }
}
