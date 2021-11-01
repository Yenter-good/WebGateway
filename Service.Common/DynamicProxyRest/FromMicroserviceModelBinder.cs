using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    /// <summary>
    /// 模式绑定实现
    /// </summary>
    public class FromMicroserviceModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelType.IsInterface)
                return Task.CompletedTask;

            var assemblyType = bindingContext.ModelType;

            bindingContext.Result = ModelBindingResult.Success(ProxyFactory.CreateByInterfaceType(assemblyType));

            return Task.CompletedTask;
        }
    }
}
