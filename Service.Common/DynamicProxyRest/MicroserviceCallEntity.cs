using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common
{
    /// <summary>
    /// 远程服务调用通用实体
    /// </summary>
    public class MicroserviceCallEntity
    {
        /// <summary>
        /// 程序集名
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 参数类型列表
        /// </summary>
        public List<string> ArgTypeNames { get; set; } = new List<string>();
        /// <summary>
        /// 参数值列表
        /// </summary>
        public List<string> ArgValues { get; set; } = new List<string>();
    }
}
