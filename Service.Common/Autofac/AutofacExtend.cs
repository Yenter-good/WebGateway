using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Service.Common
{
    public static class AutofacExtend
    {
        public static void UseCustomConfigureContainer(this ContainerBuilder containerBuilder)
        {
            var baseRootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var filePaths = Directory.GetFiles(baseRootPath, "*.dll", SearchOption.TopDirectoryOnly).Where(p => Path.GetFileNameWithoutExtension(p).Contains("WebAPI"));

            var assemblies = filePaths.Select(Assembly.LoadFrom).Distinct().ToArray();

            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            containerBuilder.BuildSingleton(assemblies)
                            .BuildScope(assemblies)
                            .BuildNamed(types)
                            .BuildController(types)
                            .CacheAssemblies(types);
        }

        /// <summary>
        /// 缓存当前程序集当中的所有类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        private static ContainerBuilder CacheAssemblies(this ContainerBuilder builder, List<Type> types)
        {
            foreach (var type in types)
            {
                ServiceCommonParam.CurrentAssembliesCache.Add(type);
            }
            return builder;
        }

        /// <summary>
        /// 注册单例服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        private static ContainerBuilder BuildSingleton(this ContainerBuilder builder, Assembly[] assemblies)
        {
            var singletonType = typeof(ISingletonService);
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => singletonType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            return builder;
        }

        /// <summary>
        /// 注册作用域服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static ContainerBuilder BuildScope(this ContainerBuilder builder, Assembly[] assemblies)
        {
            var scopeType = typeof(IScopeService);
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => scopeType.IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            return builder;
        }

        /// <summary>
        /// 注册单接口多实现的服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static ContainerBuilder BuildNamed(this ContainerBuilder builder, List<Type> types)
        {
            var singletonType = typeof(ISingletonService);

            var classTypes = types.Where(t => t.GetTypeInfo().GetCustomAttribute<ServiceIdAttribute>() != null);
            foreach (var classType in classTypes)
            {
                var serviceId = classType.GetTypeInfo().GetCustomAttribute<ServiceIdAttribute>();
                //对ServiceId不为空的对象，找到第一个继承IService的接口并注入接口及实现
                var interfaceObjs = classType.GetInterfaces();

                if (interfaceObjs != null && interfaceObjs.Length > 0)
                {
                    var interfaceObj = interfaceObjs.FirstOrDefault(p => singletonType.IsAssignableFrom(p));

                    if (interfaceObj != null)
                        builder.RegisterType(classType).Named(serviceId.Id, interfaceObj).SingleInstance();
                    else
                        builder.RegisterType(classType).Named(serviceId.Id, interfaceObj).InstancePerLifetimeScope();
                }
            }

            return builder;
        }

        private static ContainerBuilder BuildController(this ContainerBuilder builder, List<Type> types)
        {
            var controllerType = typeof(ControllerBase);

            var classTypes = types.Where(t => controllerType.IsAssignableFrom(t));
            foreach (var classType in classTypes)
                builder.RegisterType(classType)
                       .InstancePerLifetimeScope()
                       .OnActivating(ActivatingHandler);

            return builder;
        }

        private static void ActivatingHandler(IActivatingEventArgs<object> handler)
        {
            var fieldInfos = handler.Instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                var fromservice = fieldInfo.GetCustomAttribute<FromServiceAttribute>();
                object service = null;
                if (fromservice != null)
                {
                    if (string.IsNullOrEmpty(fromservice.Id))
                        service = handler.Context.Resolve(fieldInfo.FieldType);
                    else
                        service = handler.Context.ResolveNamed(fromservice.Id, fieldInfo.FieldType);
                }

                var frommicroservice = fieldInfo.GetCustomAttribute<FromMicroserviceAttribute>();
                if (frommicroservice != null)
                    service = ProxyFactory.CreateByInterfaceType(fieldInfo.FieldType);

                fieldInfo.SetValue(handler.Instance, service);
            }
        }
    }
}
