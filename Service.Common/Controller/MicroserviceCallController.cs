using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Autofac;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Service.Common
{
    [ApiController]
    [Route("[controller]")]
    public class MicroserviceCallController : ControllerBase
    {
        private static ConcurrentDictionary<string, object> _classCache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 接受远端调用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [HttpPost]
        public MicroserviceResultEntity Index([FromBody] MicroserviceCallEntity entity, [FromServices] ILifetimeScope context)
        {
            Type classObjType = null;
            object classObj = null;

            //判断远端调用的接口名是否已经缓存
            //没有缓存从当前程序集缓存中找到指定接口,然后从容器中找到指定类型
            if (_classCache.ContainsKey(entity.ClassName))
                classObj = _classCache[entity.ClassName];
            else
            {
                var interfaceObj = ServiceCommonParam.CurrentAssembliesCache.Find(p => p.FullName == entity.ClassName);
                if (interfaceObj == null)
                    return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.NotFindInterface };

                if (!context.IsRegistered(interfaceObj))
                    return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.NotFindInterface };
                classObj = context.Resolve(interfaceObj);
            }

            if (classObj == null)
                return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.NotFindClass };

            if (!_classCache.ContainsKey(entity.ClassName))
                _classCache[entity.ClassName] = classObj;

            classObjType = classObj.GetType();

            //获取远端调用的类型的所有方法,通过远端调用传来的方法名进行匹配
            //获取远端调用传来的参数列表,对比现有的方法的参数列表,找到匹配的那一个方法
            var allMethods = classObjType.GetMethods();
            var methods = allMethods.Where(p => p.Name == entity.MethodName).ToArray();
            if (methods == null || methods.Length == 0)
                return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.NotFindMethod };

            MethodInfo targetMethod = null;
            Type[] paramTypes = null;
            foreach (var method in methods)
            {
                var param = method.GetParameters();
                if (param.Length == entity.ArgTypeNames.Count)
                {
                    bool hit = true;
                    for (int i = 0; i < param.Length; i++)
                    {
                        if (param[i].ParameterType.Name != entity.ArgTypeNames[i])
                        {
                            hit = false;
                            break;
                        }
                    }
                    if (hit)
                    {
                        targetMethod = method;
                        paramTypes = param.Select(p => p.ParameterType).ToArray();
                    }
                }
            }

            if (targetMethod == null)
                return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.NotFindMethod };

            //反射调用方法,判断方法的返回值是不是Task类型,如果是,那么反射获取返回值里的Result属性,传回调用方
            try
            {
                object[] args = new object[paramTypes.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    args[i] = JsonConvert.DeserializeObject(entity.ArgValues[i], paramTypes[i]);
                }

                var result = targetMethod.Invoke(classObj, args);

                if (targetMethod.ReturnType.Name.Contains("Task"))
                {
                    var data = JObject.FromObject(result)["Result"];
                    return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.Success, Content = JsonConvert.SerializeObject(data) };
                }
                else
                    return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.Success, Content = JsonConvert.SerializeObject(result) };

            }
            catch (Exception ex)
            {
                return new MicroserviceResultEntity() { StatusCode = MicroserviceResultStatusCode.ServiceError, ErrorMsg = ex.Message };
            }
        }

    }
}
