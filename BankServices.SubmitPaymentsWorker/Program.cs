using BankServices.Interfaces;
using BankServices.SubmitPaymentsWorker.Handlers;
using BankServies.PaymentServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rebus.Config;
using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Routing.TransportMessages;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BankServices.SubmitPaymentsWorker
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting worker host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    string uri, defaultQueue;
                    using (var provider = services.BuildServiceProvider())
                    {
                        var config = provider.GetRequiredService<IConfiguration>();

                        uri = config.GetValue<string>("PaymentsMessageBroker:Uri");
                        defaultQueue = config.GetValue<string>("PaymentsMessageBroker:DefaultQueue");
                    }

                    services.AddRebusHandler<SubmitPaymentHandler>();

                    services.AddTransient<IAquiringBankPaymentService, AquiringBankPaymentService>();

                    // Configure and register Rebus
                    services.AddRebus(configure => configure
                        .Logging(l => l.Serilog())
                        .Transport(t => t.UseRabbitMq(uri, defaultQueue))
                        .Routing(r => r.TypeBased()));

                    services.BuildServiceProvider().UseRebus();
                });
    }
}
