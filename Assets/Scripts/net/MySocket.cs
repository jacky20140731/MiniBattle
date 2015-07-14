using System.Collections;
using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using com.tianhe.account.module.account.model;
using UnityEngine;
namespace com.tianhe.net
{
    public class MySocket
    {
        /// <summary>
        /// 消息sessionid
        /// </summary>
        private long _sessionid = 0;
        public long sessionid
        {
            get { return _sessionid; }
            set { this._sessionid = value; }
        }
        /// <summary>
        /// 消息序号（默认1，每次自增）
        /// </summary>
        private int _messageIndex = 0;
        public int messageIndex
        {
            get { return this._messageIndex; }
            set { this._messageIndex = value; }
        }
        //Socket客户端对象
        private Socket clientSocket;
        /// <summary>
        /// 启动线程
        /// </summary>
        Thread thread;
        //单例模式
        private static MySocket instance;

        public static MySocket GetInstance()
        {
            if (instance == null)
            {
                instance = new MySocket();
            }
            return instance;
        }

        MySocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务器IP地址
            IPAddress ipAddress = IPAddress.Parse("192.168.199.214");
            //服务器端口
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, 22222);
            //这是一个异步的建立连接，当连接建立成功时调用connectCallback方法
            IAsyncResult result = clientSocket.BeginConnect(ipEndpoint, new AsyncCallback(connectCallback), clientSocket);
            //这里做一个超时的监测，当连接超过5秒还没成功表示超时
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                Closed();
                Console.WriteLine("connect Time Out");
            }
            else
            {
                thread = new Thread(new ThreadStart(ReceiveSorket));
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void connectCallback(IAsyncResult ir)
        {
            Debug.Log("connectSuccess");
        }

        private void ReceiveSorket()
        {
            while (true)
            {
                if (!clientSocket.Connected)
                {
                    //与服务器断开连接跳出循环
                    Console.WriteLine("Failed to clientSocket server.");
                    clientSocket.Close();
                    break;
                }
                try
                {
                    byte[] bytes = new byte[24];
                    //如果没有回发会一直在这里等着。
                    int i = clientSocket.Receive(bytes);
                    if (i <= 0)
                    {
                        clientSocket.Close();
                        break;
                    }
                    ByteBuffer bb = new ByteBuffer(bytes);
                    for (Message message = decodeMessage(bb); message != null; message = decodeMessage(bb))
                    {
                        MessageManager.messageManager.addNewCallBackMessage(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to clientSocket error." + e);
                    clientSocket.Close();
                    break;
                }
            }
        }

        //向服务端发送一条字符串
        public void SendMessage(Message message)
        {
            byte[] msg = encodeMessage(message);

            if (!clientSocket.Connected)
            {
                clientSocket.Close();
                return;
            }
            try
            {
                IAsyncResult asyncSend = clientSocket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(sendCallback), clientSocket);
                bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
                if (!success)
                {
                    clientSocket.Close();
                    Console.WriteLine("Failed to SendMessage server.");
                }
            }
            catch
            {
                Console.WriteLine("send message error");
            }
        }

        private void sendCallback(IAsyncResult asyncSend)
        {
            Debug.Log("send message success!");
        }

        //关闭Socket
        public void Closed()
        {
            thread.Abort();
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            clientSocket = null;
        }

        ByteBuffer readBuffer = new ByteBuffer();
        public Message decodeMessage(ByteBuffer bb)
        {
            if (bb == null || bb.available() <= 0) return null;
            readBuffer.writeByteBuffer(bb);
            int len = readBuffer.readShort();
            if (len > readBuffer.available())
            {
                readBuffer.rPos(readBuffer.rPos() - 2);
                return null;
            }
            ByteBuffer data = new ByteBuffer();
            data.writeByteBuffer(readBuffer);
            Message message = new Message(data,len-20);
            readBuffer.compack(len - 1);
            return message;
        }

        public byte[] encodeMessage(Message message)
        {
            if (message == null)
                return null;
            return message.data.getBytes();
        }

    }
}