using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Service.Common
{
    public static class ServiceProjectExtend
    {
        /// <summary>
        /// 注入服务项目所需的服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddServiceProject(this IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
            services.AddSingleton(serviceProvider =>
            {
                var server = serviceProvider.GetRequiredService<IServer>();
                return server.Features.Get<IServerAddressesFeature>();
            });
            services.AddSwaggerGen();
            services.AddHttpClient();
        }

        /// <summary>
        /// 配置服务项目所需的配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="address"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseServiceProject(this IApplicationBuilder builder, IServerAddressesFeature address, IHttpClientFactory client)
        {
            return builder.UseConsul(address).UseSwaggerUI().UseHTTPClient(client);
        }
    }
}
