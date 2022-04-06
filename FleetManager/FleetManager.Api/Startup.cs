using FleetManager.Common.Enums;
using FleetManager.Data.Infrastructure;
using FleetManager.Dto.Common;
using FleetManager.Service.DeliveryPoint;
using FleetManager.Service.Shipment;
using FleetManager.Service.Vehicle;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace FleetManager.Api
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
            #region Dependency Injections

            services.AddTransient<IRedisProvider, RedisProvider>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IDeliveryPointService, DeliveryPointService>();
            services.AddTransient<IWrongDeliveryService, WrongDeliveryService>();
            services.AddTransient<BagService>();
            services.AddTransient<PackageService>();
            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                return key switch
                {
                    ShipmentType.Bag => serviceProvider.GetService<BagService>(),
                    ShipmentType.Package => serviceProvider.GetService<PackageService>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            #endregion

            var assembly = AppDomain.CurrentDomain.Load("FleetManager.Service");
            services.AddMediatR(assembly);

            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = Configuration.GetConnectionString("redis");
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FleetManager.Api", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FleetManager.Api v1"));
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResultDto<bool>(ResultStatus.Exception)));
                });
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
