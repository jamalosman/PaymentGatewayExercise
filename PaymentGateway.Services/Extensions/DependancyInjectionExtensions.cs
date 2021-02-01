using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Services.Extensions
{
    public static class DependancyInjectionExtensions
    {

        public static IServiceCollection AddPaymentGatewayServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IPaymentService, PaymentService>()
                .AddTransient<ISecurityService, SecurityService>();
        }

    }
}
