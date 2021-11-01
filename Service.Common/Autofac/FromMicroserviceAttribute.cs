using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Common.Autofac
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FromMicroserviceAttribute : FromServiceAttribute
    {
    }
}
