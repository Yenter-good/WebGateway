using SocketCommon;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Service.Common
{
    public class SocketHelper
    {


    }

    public class SocketResult
    {
        public static SocketResult Ok(string content)
        {
            return new SocketResult()
            {
                Content = content,
                Success = true
            };
        }

        public static SocketResult Fail(string errMsg)
        {
            return new SocketResult()
            {
                ErrorMsg = errMsg,
                Success = false
            };
        }

        public bool Success { get; set; }
        public string Content { get; private set; }
        public string ErrorMsg { get; set; }
    }
}
