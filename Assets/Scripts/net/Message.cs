using Assets.com.tianhe.account.module.account.model;
using com.tianhe.account.module.account.model;
using com.tianhe.protobuf;
using System;
using System.Collections;
namespace com.tianhe.net
{
    /// <summary>
    /// 消息
    /// </summary>
    public class Message
    {
        public delegate void EventHandle(object protoBuffer);
        public EventHandle eventHandle;

        #region field
        /// <summary>
        /// 消息总长度
        /// </summary>
        int capacity;
        /// <summary>
        /// sessionid
        /// </summary>
        long sessionid;
        /// <summary>
        /// 消息序列号(默认1,自增)
        /// </summary>
        int index;
        /// <summary>
        /// 状态码
        /// </summary>
        short status;
        /// <summary>
        /// 总模块端口
        /// </summary>
        public short cmd;
        /// <summary>
        /// 模块端口对应的分支
        /// </summary>
        public short cmd_branch;
        /// <summary>
        /// 业务内容
        /// </summary>
        public object protoBuffer;
        /// <summary>
        /// 数据缓冲
        /// </summary>
        public ByteBuffer data;
        #endregion

        #region construct
        public Message() 
        {
        }
        public Message(ByteBuffer data,int capacity)
        {
            this.data = data;
            this.capacity = capacity;
            decodeMessage();
        }
        public Message(short cmd, short cmd_branch, object protoBuffer,EventHandle handle=null)
        {
            this.cmd = cmd;
            this.cmd_branch = cmd_branch;
            this.protoBuffer = protoBuffer;
            this.eventHandle = handle;
            encodeMessage();
        }
        #endregion

        public void decodeMessage()
        {
            sessionid = data.readLong();
            index = data.readInt();
            status = data.readShort();
            cmd = data.readShort(); 
            cmd_branch = data.readShort();
            int rpos = data.rPos();
            byte[] bytes = data.compack(rpos, capacity);
            branchObject(cmd,cmd_branch,bytes);
        }

        public void encodeMessage()
        {
            byte[] bytes=null;
            bytes = PBCommon.serialize(protoBuffer);
            data = new ByteBuffer();
            data.writeShort(bytes.Length + 20);
            data.writeLong(MySocket.GetInstance().sessionid);
            data.writeInt(MySocket.GetInstance().messageIndex++);
            data.writeShort(0);
            data.writeShort(cmd);
            data.writeShort(cmd_branch);
            data.compack(0, 20,false);
            data.writeBytes(bytes);
            data.compack(0, bytes.Length + 20, false);
        }

        public object branchObject(int cmd, int cmd_branch,byte[] bytes)
        {
            object result = null;
            if (cmd == 1)
            {
                switch (cmd_branch)
                {
                    case 1:
                        result = typeof(CreateResult);
                        protoBuffer = PBCommon.deserialze<CreateResult>(bytes);
                        break;
                    case 2:
                        result = typeof(LoginResult);
                        protoBuffer = PBCommon.deserialze<LoginResult>(bytes);
                        break;
                    case 3:
                        result = typeof(ChangeResult);
                        protoBuffer = PBCommon.deserialze<ChangeResult>(bytes);
                        break;
                }
            }
            Console.WriteLine("获得返回数据:" + protoBuffer.GetType());
            return result;
        }
        /// <summary>
        /// 时间回调处理
        /// </summary>
        public void handle()
        {
            if (eventHandle != null)
                eventHandle(protoBuffer);
        }
    }
}
