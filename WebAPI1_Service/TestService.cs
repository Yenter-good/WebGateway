using Service.Common;
using System;
using System.Threading.Tasks;
using WebAPI1_Interface;

namespace WebAPI1_Service
{
    public class TestService : ITestService
    {
        public Task<string> GetString()
        {
            return Task.FromResult("测试WebAPI1");
        }
    }
}
