using PaymentGateway.Domain.Models;
using PaymentGateway.Services.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<IEnumerable<Claim>> GetClaims(string apiKey);
    }
}
