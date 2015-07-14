using System.Collections;
using ProtoBuf;
using System.IO;
using System;
using com.tianhe.account.module.account.model;
using com.tianhe.net;
namespace com.tianhe.protobuf
{
    public class PBTest
    {

        // Use this for initialization
        public void Start()
        {
            Address address = new Address();
            address.line1 = "line1";
            address.line2 = "line2";
            using (var file = File.Create("Address.bin"))
            {
                ProtoBuf.Serializer.Serialize(file, address);
            }

            Address binP = null;
            using (var file = File.OpenRead("Address.bin"))
            {
                binP = ProtoBuf.Serializer.Deserialize<Address>(file);
            }
            Console.WriteLine(binP.line1 + "," + binP.line2);

            Address add = new Address();
            add.line1 = "1";
            add.line2 = "2";
            byte[] bytes = PBCommon.serialize<Address>(add);
            Address add1 = PBCommon.deserialze<Address>(bytes);
            Console.WriteLine(add1.line1 + "," + add1.line2);

            CreateVo vo = new CreateVo("test", "123", 3, true, 0);
            byte[] bb = PBCommon.serialize<CreateVo>(vo);
            CreateVo vo1 = PBCommon.deserialze<CreateVo>(bb);
            Console.WriteLine(vo1.name);

            CreateResult cr = new CreateResult();
            cr.code = 1;
            CreateResult cr1 = PBCommon.deserialze<CreateResult>(PBCommon.serialize<CreateResult>(cr));
            Console.WriteLine(cr1.code);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}