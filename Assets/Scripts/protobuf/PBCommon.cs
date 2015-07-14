using System.Collections;
using ProtoBuf;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace com.tianhe.protobuf
{
    /// <summary>
    /// protobuf序列化和反序列化
    /// @author jacky
    /// 2015.6.16
    /// </summary>
    public class PBCommon
    {
        public static byte[] serialize<T>(T instanse)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, instanse);
                bytes = new byte[ms.Position];
                Array.Copy(ms.GetBuffer(), bytes, bytes.Length);
            }

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    IFormatter formatter = new BinaryFormatter();
            //    formatter.Serialize(ms, instanse);
            //    bytes = ms.GetBuffer();
            //    ms.Close();
            //}

            return bytes;
        }
        public static T deserialze<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                T t = ProtoBuf.Serializer.Deserialize<T>(ms);
                return t;
            }
        }
    }
}
