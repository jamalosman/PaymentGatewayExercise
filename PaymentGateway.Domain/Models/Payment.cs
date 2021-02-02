using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public Guid? BankPaymentId { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Processed { get; set; }
        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardFullName { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardExpiry { get; set; }
    }
}
