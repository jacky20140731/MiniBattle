using System.Collections;
using ProtoBuf;
namespace com.tianhe.protobuf
{
    [ProtoContract]
    public class Address
    {
        [ProtoMember(1)]
        public string line1;
        [ProtoMember(2)]
        public string line2;
    }
}