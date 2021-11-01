using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Common
{
    public static class ConsulHelper
    {
        private static ConsulClient _client;
        private static Dictionary<string, int> _roundRobinIndex = new Dictionary<string, int>();

        /// <summary>
        /// 获取指定节点注册的地址,使用轮询策略
        /// </summary>
        /// <param name="consulKey"></param>
        /// <returns></returns>
        public static ConsulAddress GetAddress(string consulKey)
        {
            var consulAddress = ConfigHelper.Instance["Consul:Address"];

            int lastIndex = 0;
            if (_roundRobinIndex.ContainsKey(consulKey))
                lastIndex = _roundRobinIndex[consulKey];

            var consulUri = new Uri(consulAddress);

            if (_client == null)
                _client = new ConsulClient();

            _client.Config.Address = consulUri;
            var response = _client.Health.Service(consulKey).Result.Response;

            AgentService service;

            if (lastIndex >= response.Length)
                lastIndex = 0;

            service = response[lastIndex].Service;
            _roundRobinIndex[consulKey] = ++lastIndex;

            return new ConsulAddress(service.Address, service.Port);
        }
    }

    public class ConsulAddress
    {
        public ConsulAddress(string address, int port)
        {
            Address = address;
            Port = port;
        }

        public string Address { get; set; }
        public int Port { get; set; }
    }
}
