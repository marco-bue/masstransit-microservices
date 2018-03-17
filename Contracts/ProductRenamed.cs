using System;

namespace Contracts
{
    public class ProductRenamed
    {
        public ProductRenamed(string productId, string name)
        {
            ProductId = productId;
            Name = name;
        }

        public string ProductId { get; private set; }

        public string Name { get; private set; }
    }
}
