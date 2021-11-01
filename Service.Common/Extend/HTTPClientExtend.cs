using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Service.Common
{
    public static class HTTPClientExtend
    {
        public static IApplicationBuilder UseHTTPClient(this IApplicationBuilder builder, IHttpClientFactory client)
        {
            var helper = new HTTPHelper(client);
            return builder;
        }
    }
}
