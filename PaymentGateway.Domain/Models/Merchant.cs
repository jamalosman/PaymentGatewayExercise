﻿namespace PaymentGateway.Domain.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string EmailAddress { get; set; }
        public string ApiKey { get; set; }
        public string EncryptionKey { get; set; }
    }
}