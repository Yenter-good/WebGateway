using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using WebAPI2_Interface;

namespace WebAPI2_Service
{
    public class TestService1 : ITestService1
    {
        public Task<TestService1Entity> GetString(string a, int b, List<string> c, DateTime d, DataTable e, Dictionary<string, string> f, TestService1Entity entity)
        {
            return Task.FromResult(new TestService1Entity()
            {
                Name = "ceshi "
            });
        }

        public Task<int> GetString(TimeSpan ts, string a = "123")
        {
            return Task.FromResult(1);
        }
        public int? GetString()
        {
            return 33;
        }
        public T GetString<T>(T a)
        {
            return a;
        }
    }
}
