using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public class DynamicProxyInterceptor : IInterceptor
    {
        /// <summary>
        /// 远程调用会触发当前拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            //生成远程调用信息
            //命名空间、类名、方法名、参数类型列表、参数值列表
            MicroserviceCallEntity entity = new MicroserviceCallEntity();

            var assemblyName = invocation.Method.DeclaringType.Assembly.GetName().Name;

            ConsulAddress url;

            if (ConfigHelper.Instance["MicroserviceCall:EnableConsul"] == "1")
            {
                var consulKey = ConfigHelper.Instance[$"MicroserviceCall:MicroserviceMapper:{assemblyName}"];
                url = ConsulHelper.GetAddress(consulKey);
            }
            else
            {
                var address = ConfigHelper.Instance[$"MicroserviceCall:MicroserviceUrls:{assemblyName}:Address"];
                var port = int.Parse(ConfigHelper.Instance[$"MicroserviceCall:MicroserviceUrls:{assemblyName}:Port"]);
                url = new ConsulAddress(address, port);
            }

            entity.Namespace = invocation.Method.DeclaringType.Namespace;
            entity.ClassName = invocation.Method.DeclaringType.FullName;
            entity.MethodName = invocation.Method.Name;

            foreach (var arg in invocation.Arguments)
            {
                entity.ArgTypeNames.Add(arg.GetType().Name);
                entity.ArgValues.Add(JsonConvert.SerializeObject(arg));
            }

            //通过consul获得远程调用的地址,然后调用
            var response = HTTPHelper.Post($"http://{url.Address}:{url.Port}/MicroserviceCall", JsonConvert.SerializeObject(entity));

            var type = invocation.Method.ReturnType;
            if (response.IsSuccess)
            {
                //如果判断返回值为Task,那么创建手动创建一个Task返回
                var result = JsonConvert.DeserializeObject<MicroserviceResultEntity>(response.Content);

                if (result.StatusCode != MicroserviceResultStatusCode.Success)
                    throw new EntryPointNotFoundException(this.GetEnumDescription(result.StatusCode));

                if (type.Name.Contains("Task"))
                {
                    var obj = JsonConvert.DeserializeObject("{}", type);

                    var method = obj.GetType().GetMethod("TrySetResult", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                    var args = type.GetGenericArguments();
                    if (args == null || args.Length == 0)
                    {
                        invocation.ReturnValue = null;
                        return;
                    }

                    method.Invoke(obj, new object[] { JsonConvert.DeserializeObject(result.Content, args[0]) });
                    invocation.ReturnValue = obj;
                }
                else
                    invocation.ReturnValue = JsonConvert.DeserializeObject(result.Content, invocation.Method.ReturnType);

            }
            else
                invocation.ReturnValue = type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
