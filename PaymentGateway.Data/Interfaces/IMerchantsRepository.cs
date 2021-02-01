using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Interfaces
{
    public interface IMerchantsRepository
    {
        Task<Merchant> GetMerchantByApiKey(string apiKey);
    }
}
