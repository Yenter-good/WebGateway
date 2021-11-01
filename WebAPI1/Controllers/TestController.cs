using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI1_Interface;
using WebAPI2_Interface;

namespace WebAPI1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [FromService] private ITestService _testService;
        [FromMicroservice] private ITestService1 _testService1;

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> GetString()
        {
            Console.WriteLine(DateTime.Now + "被调用");
            //var tmp = _testService1.GetString("1", 2, new List<string>() { "1", "2" }, DateTime.Now, new System.Data.DataTable(), new Dictionary<string, string>() { { "1", "2" }, { "3", "4" } }, new TestService1Entity()
            //{
            //    Name = "测试",
            //    Age = 1
            //}).GetAwaiter().GetResult();

            //var tmp1 = _testService1.GetString(TimeSpan.Zero, "321");
            //var tmp2 = _testService1.GetString();
            //var tmp3 = _testService1.GetString();

            //return tmp.Name;

            return await _testService.GetString();
        }
    }
}
