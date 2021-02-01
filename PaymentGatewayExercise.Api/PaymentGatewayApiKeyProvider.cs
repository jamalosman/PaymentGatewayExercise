using AspNetCore.Authentication.ApiKey;
using PaymentGateway.Api;
using PaymentGateway.Services.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentGateway
{
    internal class PaymentGatewayApiKeyProvider : IApiKeyProvider
    {
        private readonly ISecurityService _securityService;

        public PaymentGatewayApiKeyProvider(ISecurityService securityService)
        {
            _securityService = securityService;
        }
        public async Task<IApiKey> ProvideAsync(string key)
        {
            var claims = await _securityService.GetClaims(key);
            if (!claims.Any()) return null;

            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            return new ApiKey(key, name, claims.ToList());
        }
    }
}