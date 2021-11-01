using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common
{
    /// <summary>
    /// 单接口多实现
    /// </summary>
    public class ServiceIdAttribute : Attribute
    {
        public ServiceIdAttribute(string id)
        {
            Id = id;
        }

        public string Id { get; }
    }
}
