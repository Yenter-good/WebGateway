using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common
{
    /// <summary>
    /// 从远程终端中获取服务支持
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field)]
    public class FromMicroserviceAttribute : Attribute, IBinderTypeProviderMetadata, IBindingSourceMetadata, IModelNameProvider
    {
        private readonly ModelBinderAttribute modelBinderAttribute = new ModelBinderAttribute() { BinderType = typeof(FromMicroserviceModelBinder) };

        public Type BinderType => modelBinderAttribute.BinderType;

        public BindingSource BindingSource => modelBinderAttribute.BindingSource;

        public string Name => modelBinderAttribute.Name;
    }
}
