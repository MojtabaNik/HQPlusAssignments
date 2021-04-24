using Hangfire;
using Hangfire.MemoryStorage;
using HQPlusAssignments.Application.Core.Settings;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Application.System;
using HQPlusAssignments.Common.Extensions;
using HQPlusAssignments.Services.Api.Infra.MiddleWares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;

namespace HQPlusAssignments.Services.Api
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
            services.AddControllers();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "HQPlusAssignments.Services.Api", Version = "v1" }));

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            //Set Mail Settings From Config
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            //Add Dependancies With Reflection
            services.AddSingletonsByConvention(
                typeof(IFileService).Assembly,
                typeof(FileService).Assembly,
                x => x.Name.EndsWith("Service"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireDashboard();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HQPlusAssignments.Services.Api v1"));
            }

            //Api UserFriendly,UnhandledException handling
            app.UseExceptionAndResponseMessageHandling();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void Job()
        {
            Debug.WriteLine($"Done ! {DateTime.Now}");
        }
    }
}
