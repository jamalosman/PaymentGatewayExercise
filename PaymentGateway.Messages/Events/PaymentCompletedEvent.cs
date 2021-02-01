using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Messages.Events
{
    public class PaymentCompletedEvent
    {
        public int PaymentId { get; set; }
        public Guid BankPaymentId { get; set; }
        public bool Success { get; set; }

    }
}
