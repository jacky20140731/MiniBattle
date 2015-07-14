using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.com.tianhe.account.module.account.model
{
    /// <summary>
    /// 登录结果
    /// </summary>
    [ProtoContract]
    public class LoginResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        [ProtoMember(1)]
        public int code;
        /// <summary>
        /// 游戏服id
        /// </summary>
        [ProtoMember(2)]
        public int serverid;
        /// <summary>
        /// 游戏服ip
        /// </summary>
        [ProtoMember(3)]
        public string ip;
        /// <summary>
        /// 游戏服端口
        /// </summary>
        [ProtoMember(4)]
        public int port;
        /// <summary>
        /// 新的登录token
        /// </summary>
        [ProtoMember(5)]
        public string token;
        public LoginResult() { }
        public LoginResult(int code, int serverid, string ip, int port, string token)
        {
            this.code = code;
            this.serverid = serverid;
            this.ip = ip;
            this.port = port;
            this.token = token;
        }
    }
}
