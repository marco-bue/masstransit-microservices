using System;
using System.Collections.Generic;

namespace OrderService.Domain
{
    public class Order
    {
        private readonly string id;

        private IList<OrderLine> orderLines;

        private Address address;

        // We skip stuff like Customer here, you get the idea.
        public Order(Address address, IList<OrderLine> orderLines)
        {
            id = $"ORD-{Guid.NewGuid().ToString().Substring(0, 4)}";
            if (orderLines == null)
            {
                throw new ArgumentNullException(nameof(orderLines));
            }

            if (orderLines.Count < 1)
            {
                throw new ArgumentException("An order may not be empty", nameof(orderLines));
            }

            this.orderLines = orderLines;
            this.address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public string Id => id;

        public Address Address => address;

        public IList<OrderLine> OrderLines => orderLines;
    }
}
