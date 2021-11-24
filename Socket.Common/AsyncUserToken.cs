using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SocketCommon
{
    public class AsyncUserToken
    {
        /// <summary>  
        /// 客户端IP地址  
        /// </summary>  
        public IPAddress IPAddress { get; set; }

        /// <summary>  
        /// 远程地址  
        /// </summary>  
        public EndPoint Remote { get; set; }

        /// <summary>  
        /// 通信SOKET  
        /// </summary>  
        public Socket Socket { get; set; }

        /// <summary>  
        /// 连接创建时间  
        /// </summary>  
        public DateTime ConnectTime { get; set; }

        /// <summary>  
        /// 数据缓存区  
        /// </summary>  
        public List<byte> Buffer { get; set; }

        /// <summary>
        /// 已经开始接受数据
        /// </summary>
        public bool BeginAccept { get; set; }

        /// <summary>
        /// 当前会话包总长度
        /// </summary>
        public int PackageLength { get; set; }

        public AsyncUserToken()
        {
            this.Buffer = new List<byte>();
        }
    }
}
