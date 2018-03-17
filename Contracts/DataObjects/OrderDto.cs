using System.Collections.Generic;

namespace Contracts.DataObjects
{
    public class OrderDto
    {
        public string Id { get; set; }

        public AddressDto Address { get; set; }

        public List<OrderLineDto> OrderLines { get; set; }
    }
}
