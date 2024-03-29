﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;
using Web.Models;
using Web.DBEntities;

namespace Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FantasyLeague;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
           // var connection = @"Server=(localdb)\MSSQLLocalDB;Database=FantasyLeague;Trusted_Connection=True;";
       services.AddDbContext<FantasyLeagueContext>(options => options.UseSqlServer(connection));
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddDistributedMemoryCache();
            services.AddScoped<IDBReader, DBReader>();

            services.AddSession(
                options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(6000);
                    options.CookieName = ".MyCoreApp";
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}/{key?}");
                routes.MapRoute(
    name: "competitions",
    template: "{action}/{id?}/{key?}",
    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "CatchAll",
                    template: "{*any}",

                    defaults: new {controller = "Home", action = "Error"}

                );
            });

            
        }
    }
}
