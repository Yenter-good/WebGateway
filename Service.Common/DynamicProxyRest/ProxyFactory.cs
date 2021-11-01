using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Service.Common
{
    /// <summary>
    /// 远程服务调用代理生成工厂
    /// </summary>
    public class ProxyFactory
    {
        private static ProxyGenerator _generator = new ProxyGenerator(); //实例化【代理类生成器】  

        public static object CreateByInterfaceType(Type type, IInterceptor interceptor = null)
        {
            if (type == null)
            {
                throw new Exception("类型不能为空");
            }
            if (!type.IsInterface)
            {
                throw new Exception("类型不是接口");
            }
            String key = type.FullName;
            if (ServiceCommonParam.MicroserviceCallProxyCache.ContainsKey(key))
                return ServiceCommonParam.MicroserviceCallProxyCache[key];

            if (interceptor == null)
                interceptor = new DynamicProxyInterceptor();

            object obj = _generator.CreateInterfaceProxyWithoutTarget(type, interceptor);

            ServiceCommonParam.MicroserviceCallProxyCache[key] = obj;

            return obj;
        }
    }
}
