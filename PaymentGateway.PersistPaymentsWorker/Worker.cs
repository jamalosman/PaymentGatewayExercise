using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.Messages.Events;
using Rebus.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.PersistPaymentsWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;

        public Worker(ILogger<Worker> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.Subscribe<PaymentCompletedEvent>();

            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
        }
    }
}
