using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common
{
    public static class CustomHostBuilder
    {
        public static IHostBuilder UseCustomHostBuilder(this IHostBuilder host)
        {
            var result = host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//使用autofac的容器工厂替换系统默认的容器

            return result;
        }
    }
}
