using System;
namespace OrderService.Domain
{
    public class OrderLine
    {
        private readonly string productId;

        private readonly int amount;

        public OrderLine(string productId, int amount)
        {
            this.productId = productId ?? throw new ArgumentNullException(nameof(productId));
            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));    
            }

            this.amount = amount;
        }

        public string ProductId => productId;

        public int Amount => amount;
    }
}
