using ProtoBuf;
using System.Collections;
namespace com.tianhe.account.module.account.model
{
    /// <summary>
    /// 修改密码返回结果
    /// </summary>
    [ProtoContract]
    public class ChangeResult
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        [ProtoMember(1)]
        int code;
    }
}