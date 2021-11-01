using Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI2_Interface
{
    public interface ITestService1 : ISingletonService
    {
        Task<TestService1Entity> GetString(string a, int b, List<string> c, DateTime d, DataTable e, Dictionary<string, string> f, TestService1Entity entity);

        Task<int> GetString(TimeSpan ts, string a = "123");
        int? GetString();
        T GetString<T>(T a);
    }
}
