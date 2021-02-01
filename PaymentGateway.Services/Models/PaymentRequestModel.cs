using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.Models
{
    public class PaymentRequestModel
    {
        public int MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardFullName { get; set; }
        public string CardNumber { get; set; }
        public int CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
        public string Cvv { get; set; }
    }
}
