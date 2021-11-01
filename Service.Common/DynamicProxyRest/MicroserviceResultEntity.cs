using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Service.Common
{
    public class MicroserviceResultEntity
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public MicroserviceResultStatusCode StatusCode { get; set; }
        /// <summary>
        /// 返回值
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }

    public enum MicroserviceResultStatusCode
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Success = 0,
        /// <summary>
        /// 服务内部错误
        /// </summary>
        [Description("服务内部错误")]
        ServiceError = 1,
        /// <summary>
        /// 未找到调用的接口
        /// </summary>
        [Description("未找到调用的接口")]
        NotFindInterface = 2,
        /// <summary>
        /// 未找到调用的类型
        /// </summary>
        [Description("未找到调用的类型")]
        NotFindClass = 3,
        /// <summary>
        /// 未找到调用的方法
        /// </summary>
        [Description("未找到调用的方法")]
        NotFindMethod = 4
    }
}
