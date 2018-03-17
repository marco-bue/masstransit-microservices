using System;

namespace OrderService.Domain
{
    public class Product
    {
        private readonly string productId;

        private string name;

        public Product(string productId, string name)
        {
            if (productId == null || productId.Length != 5)
            {
                throw new ArgumentException("Invalid Product ID", nameof(productId));
            }

            this.productId = productId;
            this.name = name;
        }

        public string ProductId => productId;

        public string Name => name;

        public void Rename(string name)
        {
            this.name = name;
        }
    }
}
