using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Service.Common
{
    public static class SwaggerExtend
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection service)
        {
            return service.AddSwaggerGen(p =>
             {
                 p.SwaggerDoc(ConfigHelper.Instance["APIInfo:ServiceName"], new Microsoft.OpenApi.Models.OpenApiInfo()
                 {
                     Title = ConfigHelper.Instance["APIInfo:Desc"],
                     Version = ConfigHelper.Instance["APIInfo:Version"],
                     Description = ConfigHelper.Instance["APIInfo:Desc"]
                 });
                 var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                 var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                 p.IncludeXmlComments(xmlFile);
                 p.DocumentFilter<SwaggerHideFilter>();
             });
        }

        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            var name = ConfigHelper.Instance["APIInfo:ServiceName"];
            var version = ConfigHelper.Instance["APIInfo:Version"];
            app.UseSwagger();
            return app.UseSwaggerUI(p =>
             {
                 p.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{name}");
             });
        }

        public static void UseSwaggerUI(this IApplicationBuilder app, List<string> swaggerNames)
        {
            var name = ConfigHelper.Instance["APIInfo:ServiceName"];
            app.UseSwagger(p =>
            {
                p.RouteTemplate = "{documentName}/swagger.json";
            });
            app.UseSwaggerUI(p =>
            {
                swaggerNames.ForEach(d =>
                {
                    p.SwaggerEndpoint($"/{d}/swagger.json", $"{d}");
                });
            });
        }
    }
}
