using Service.Common;
using System;
using System.Threading.Tasks;

namespace WebAPI1_Interface
{
    public interface ITestService : ISingletonService
    {
        Task<string> GetString();
    }
}
