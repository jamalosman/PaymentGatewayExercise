using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Data.Repositories
{
    public class MerchantsRepository : IMerchantsRepository
    {
        private readonly PaymentsContext _paymentsContext;

        public MerchantsRepository(PaymentsContext paymentsContext)
        {
            _paymentsContext = paymentsContext;
            if (!_paymentsContext.Merchants.Any())
            {
                _paymentsContext.AddRange(
                    new Merchant
                    {
                        ApiKey = "84e3868bd19b48d7b2c2c1a420afca96",
                        EncryptionKey = "abcd1234$%^&8765£$%^poiu",
                        CompanyName = "PaperShop",
                        ContactName = "Ian Papier",
                        EmailAddress = "ian.papier@papershop.com",
                    },
                    new Merchant
                    {
                        ApiKey = "51453ab10b194cea9004da4501f846fc",
                        EncryptionKey = "8765£$%^poiuabcd1234$%^&",
                        CompanyName = "Peanuts R Us",
                        ContactName = "Alex Nutter",
                        EmailAddress = "alex.nutter@peanutsrus.com",
                    });
                _paymentsContext.SaveChanges();
            }
        }



        public async Task<Merchant> GetMerchantByApiKey(string apiKey)
        {
            return await _paymentsContext.Merchants.FirstOrDefaultAsync(x => x.ApiKey == apiKey);
        }

        public async Task<Merchant> GetMerchantById(int id)
        {
            return await _paymentsContext.Merchants.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
