using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayExercise.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public int MerchantId { get; set; }
        public DateTime Timestamp { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public string CardFullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardExpiry { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
