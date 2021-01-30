using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PaymentGateway.Data;
using PaymentGateway.Services.Validators;
using Rebus.ServiceProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Rebus.RabbitMq;
using Rebus.Config;
using Serilog;
using PaymentGateway.Services.Extensions;
using PaymentGateway.Data.Extensions;
using AutoMapper;
using PaymentGateway.Services.MappingProfiles;
using Microsoft.AspNetCore.Diagnostics;
using PaymentGateway.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using FluentValidation;
using Rebus.Routing.TypeBased;

namespace PaymentGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PaymentRequestValidator>());

            services.AddDbContext<PaymentsContext>(o => 
                o.UseSqlServer(Configuration.GetConnectionString("PaymentsDatabase")));


            // Configure and register Rebus
            services.AddRebus(configure => configure
                .Logging(l => l.Serilog())
                .Transport(t => t.UseRabbitMq(
                    Configuration.GetValue<string>("PaymentsMessageBroker:ConnectionString"),
                    Configuration.GetValue<string>("PaymentsMessageBroker:PaymentsQueue")
                ))
                .Routing(r => r.TypeBased()));

            services.AddLogging(cfg => cfg.AddSerilog());
            services.AddAutoMapper(typeof(PaymentMappingProfile));

            services.AddPaymentGatewayServices();
            services.AddRepositories();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseExceptionHandler(a => a.Run(async context =>
            //{
            //    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            //    var exception = feature.Error;

            //    if (exception is NotFoundException notFoundException)
            //    {
            //        context.Response.StatusCode = StatusCodes.Status404NotFound;
            //        await context.Response.WriteAsync(JsonSerializer.Serialize(new
            //        {
            //            Success = false,
            //            Message = exception.Message
            //        }));
            //    }
            //    else if (exception is ValidationException validationException)
            //    {
            //        context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //        await context.Response.WriteAsync(JsonSerializer.Serialize(new
            //        {
            //            Success = false,
            //            Message = "The request was invalid",
            //            InvalidFields = validationException.Data
            //        }));
            //    }
            //    else
            //    {
            //        app.ApplicationServices.GetService<Microsoft.Extensions.Logging.ILogger<Startup>>()
            //        .LogError(exception, exception.Message);

            //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //        await context.Response.WriteAsync(JsonSerializer.Serialize(new
            //        {
            //            Success = false,
            //            Message = "There was an error, if the isuse persists, please contact support"
            //        }));
            //    }
            //}));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.UseRebus();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSerilogRequestLogging();
        }
    }
}
