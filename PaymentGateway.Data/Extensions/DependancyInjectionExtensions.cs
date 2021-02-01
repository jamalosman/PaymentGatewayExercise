using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data.Interfaces;
using PaymentGateway.Data.Repositories;

namespace PaymentGateway.Data.Extensions
{
    public static class DependancyInjectionExtensions
    {

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddTransient<IMerchantsRepository, MerchantsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }

    }
}
