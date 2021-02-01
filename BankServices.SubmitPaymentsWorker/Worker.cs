using BankServies.PaymentServices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.Messages.Commands;
using Rebus.Bus;
using Rebus.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BankServices.SubmitPaymentsWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBus _bus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(ILogger<Worker> logger, IBus bus, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _bus = bus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.Subscribe<SubmitPaymentCommand>();

            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            _logger.LogInformation("Worker stopped at: {time}", DateTimeOffset.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
