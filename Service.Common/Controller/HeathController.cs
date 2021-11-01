using Microsoft.AspNetCore.Mvc;
using System;

namespace Service.Common
{
    /// <summary>
    /// 服务健康检测
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    [SwaggerHide]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public String check()
        {
            return "ok";
        }
    }
}