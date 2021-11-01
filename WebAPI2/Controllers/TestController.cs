using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// 测试2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetString()
        {
            Console.WriteLine(DateTime.Now + " 被调用");
            return "测试2";
        }
    }
}
