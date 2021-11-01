using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Service.Common
{
    public static class ServiceCommonParam
    {
        /// <summary>
        /// 远程服务调用的缓存
        /// </summary>
        public static ConcurrentDictionary<string, object> MicroserviceCallProxyCache = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 当前程序集所有类型的缓存
        /// </summary>
        public static List<Type> CurrentAssembliesCache = new List<Type>();
    }
}
