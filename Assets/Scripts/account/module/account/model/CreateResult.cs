using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
namespace com.tianhe.account.module.account.model
{
    [ProtoContract]
    public class CreateResult
    {
        [ProtoMember(1)]
        public int code;
    }
}
