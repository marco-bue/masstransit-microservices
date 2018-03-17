using System;

namespace Contracts
{
    public class SendOrderNotification
    {
        public SendOrderNotification(string orderId, string addressLabel, DateTime estimatedDeliveryDate)
        {
            OrderId = orderId;
            AddressLabel = addressLabel;
            EstimatedDeliveryDate = estimatedDeliveryDate;
        }

        public string OrderId { get; private set; }

        public string AddressLabel { get; private set; }

        public DateTime EstimatedDeliveryDate { get; private set; }
    }
}
