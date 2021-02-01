using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Data;
using PaymentGateway.Data.Extensions;
using PaymentGateway.PersistPaymentsWorker.Handlers;
using PaymentGateway.Services;
using PaymentGateway.Services.Extensions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Services.MappingProfiles;
using PaymentGateway.Services.Validators;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentGateway.PersistPaymentsWorker
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
                .ConfigureLogging(cfg => cfg.AddSerilog())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    string uri, defaultQueue, dbConnectionString, adminKey;
                    IConfigurationSection adminSection;
                    using (var provider = services.BuildServiceProvider())
                    {
                        var config = provider.GetRequiredService<IConfiguration>();

                        uri = config.GetValue<string>("PaymentsMessageBroker:Uri");
                        defaultQueue = config.GetValue<string>("PaymentsMessageBroker:DefaultQueue");
                        dbConnectionString = config.GetConnectionString("PaymentsDatabase");
                        adminSection = config.GetSection("AdminSettings");
                        adminKey = adminSection.GetValue<string>("AdminApiKey");
                    }


                    services.AddRebusHandler<PaymentCompletedHandler>();
                    services.AddPaymentGatewayServices();
                    services.AddRepositories();
                    services.AddDbContext<PaymentsContext>(o =>
                        o.UseSqlServer(dbConnectionString));

                    services.AddLogging(cfg => cfg.AddSerilog());
                    services.AddAutoMapper(typeof(PaymentMappingProfile));
                    services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>();
                    services.Configure<AdminSettings>(adminSection);
                    services.AddTransient<ClaimsPrincipal>(svc => {
                        var claims = svc.GetService<ISecurityService>().GetClaims(adminKey).Result;

                        return new ClaimsPrincipal(new ClaimsIdentity(claims));
                        });


                    // Configure and register Rebus
                    services.AddRebus(configure => configure
                        .Logging(l => l.Serilog())
                        .Transport(t => t.UseRabbitMq(uri, defaultQueue))
                        .Routing(r => r.TypeBased()));

                    services.BuildServiceProvider().UseRebus();
                });
    }
}
