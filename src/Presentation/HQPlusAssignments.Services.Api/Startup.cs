using HQPlusAssignments.Application.Core.HotelExtractor;
using HQPlusAssignments.Application.Core.System;
using HQPlusAssignments.Application.HotelExtractor;
using HQPlusAssignments.Application.System;
using HQPlusAssignments.Common.Extensions;
using HQPlusAssignments.Services.Api.Infra.MiddleWares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HQPlusAssignments.Services.Api", Version = "v1" });
            });

            //Add Dependancies
            var interfaceAssembly = typeof(IFileService).Assembly;
            var implementationAssembly = typeof(FileService).Assembly;

            services.AddSingletonsByConvention(interfaceAssembly, implementationAssembly, x => x.Name.EndsWith("Service"));

            //services.AddScoped<IHotelExtractorService, HotelExtractorService>();
            //services.AddScoped<IFileService, FileService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
    }
}
