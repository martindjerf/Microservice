using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Service.Clients;
using Microservice.Service.Interfaces;
using Microservice.Service.Repsitory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microservice.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", optional: false,reloadOnChange:true).AddEnvironmentVariables();
            Configuration = builder.Build(); //configuration;
        }

        public IConfiguration Configuration { get; }
        public string locationUrl;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var location = Configuration.GetSection("locationService:urlLocationService");
            services.AddMvc();
            services.AddScoped<ITeamRepository, MemoryTeamRepository>();
            services.AddSingleton<IHttpLocationClient>(new HttpLocationClient(location.Value.ToString()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseMvc();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Team service is running..");
                await context.Response.WriteAsync(string.Format("Using {0} as url for location service", locationUrl));
            });
        }
    }
}
