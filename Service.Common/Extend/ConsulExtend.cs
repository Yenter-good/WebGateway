using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Service.Common
{
    public static class ConsulExtend
    {
        /// <summary>
        /// 注册consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IServerAddressesFeature address)
        {
            var consulAddress = ConfigHelper.Instance["Consul:Address"];

            var consul = new ConsulClient(p =>
            {
                p.Address = new Uri(consulAddress);
            });

            var addr = new Uri(address.Addresses.FirstOrDefault());
            var ip = addr.Host;
            var port = addr.Port;

            var apiName = ConfigHelper.Instance["APIInfo:ServiceName"];

            var checkUrl = ConfigHelper.Instance["Consul:CheckUrl"];
            var checkIntervalStr = ConfigHelper.Instance["Consul:CheckInterval"];
            int checkInterval = 60;
            if (!int.TryParse(checkIntervalStr, out checkInterval))
                checkInterval = 60;

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromMilliseconds(1), //服务停止后多久注销
                Interval = TimeSpan.FromSeconds(checkInterval), //服务健康检查间隔
                Timeout = TimeSpan.FromSeconds(10), //检查超时的时间
                HTTP = $"http://{ip}:{port}{checkUrl}" //检查的地址
            };
            var registration = new AgentServiceRegistration()
            {
                Check = httpCheck,
                Address = ip,//本程序的IP地址
                ID = apiName + "_" + ip + ":" + port,  //服务编号，不可重复
                Name = apiName,//服务名称
                Port = port,
            };

            try
            {
                var result = consul.Agent.ServiceRegister(registration).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Consul注册失败 " + ex.Message);
            }
            return app;
        }


    }
}
