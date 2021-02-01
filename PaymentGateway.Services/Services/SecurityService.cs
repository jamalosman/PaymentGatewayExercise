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
        private readonly IEncryptionService _encryptionService;
        private readonly ApplicationConfig _settings;
        private readonly string _key = "r8aC3g@#3Vs$kiDf%fgE$y78";


        public SecurityService(IMerchantsRepository merchantsRepository, 
            IEncryptionService encryptionService,
            IOptions<ApplicationConfig> config)
        {
            _merchantsRepository = merchantsRepository;
            _encryptionService = encryptionService;
            _settings = config.Value;
            //var key1 = encryptionService.Encrypt("84e3868bd19b48d7b2c2c1a420afca96", _settings.EncryptionKeyPrefix, _key);
            //var plain1 = encryptionService.Decrypt(key1, "S0m3Pr@fiX&#", _key);
            //var key2 = encryptionService.Encrypt("51453ab10b194cea9004da4501f846fc", _settings.EncryptionKeyPrefix, _key);
            //var plain2 = encryptionService.Decrypt(key2, "S0m3Pr@fiX&#", _key);
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
