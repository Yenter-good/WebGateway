using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common
{
    /// <summary>
    /// 从本地获取服务支持
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class FromServiceAttribute : Attribute
    {
        public FromServiceAttribute()
        {
        }

        public FromServiceAttribute(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
