using Microsoft.Extensions.Diagnostics.HealthChecks;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Api.HealtChecks
{

    public class BusHealthCheck : IHealthCheck
    {
        private readonly IBus _bus;

        public BusHealthCheck(IBus bus)
        {
            _bus = bus;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _bus.SendLocal("HealthCheck message");
                return HealthCheckResult.Healthy("Bus Healthy");
            }
            catch
            {
                return HealthCheckResult.Unhealthy("Bus Unhealthy");
            }
        }
    }
}
