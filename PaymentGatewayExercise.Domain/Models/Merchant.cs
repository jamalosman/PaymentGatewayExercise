using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGatewayExercise.Domain.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
