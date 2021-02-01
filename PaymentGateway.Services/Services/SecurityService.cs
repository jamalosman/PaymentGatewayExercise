using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IMerchantsRepository _merchantsRepository;
        private readonly AdminSettings _settings;

        public SecurityService(IMerchantsRepository merchantsRepository, IOptions<AdminSettings> config)
        {
            _merchantsRepository = merchantsRepository;
            _settings = config.Value;
        }

        public async Task<IEnumerable<Claim>> GetClaims(string apiKey)
        {
            var adminKey = _settings.AdminApiKey;

            if (apiKey == adminKey)
            {
                return new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin_user"),
                    new Claim(ClaimTypes.Role, "admin"),
                };
            }

            var merchant = await _merchantsRepository.GetMerchantByApiKey(apiKey);

            if (merchant != null)
            {
                return new List<Claim>
                {
                    new Claim(ClaimTypes.Name, merchant.CompanyName),
                    new Claim(ClaimTypes.Role, "merchant"),
                    new Claim(ClaimTypes.NameIdentifier, merchant.Id.ToString())
                };
            }

            return Enumerable.Empty<Claim>();
        }
    }
}
