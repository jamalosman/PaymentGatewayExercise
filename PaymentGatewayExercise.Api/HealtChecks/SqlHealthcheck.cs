using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Api.HealtChecks
{
    public class SqlHealthcheck : IHealthCheck
    {
        private readonly PaymentsContext _paymentsContext;

        public SqlHealthcheck(PaymentsContext paymentsContext)
        {
            _paymentsContext = paymentsContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connected = await _paymentsContext.Database.CanConnectAsync();
            if (connected)
                return HealthCheckResult.Healthy("SQL Server Healthy");
            else return HealthCheckResult.Unhealthy("SQL Server Unhealthy");
        }
    }
}
