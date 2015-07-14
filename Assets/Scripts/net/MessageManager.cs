using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 消息管理器
/// </summary>
namespace com.tianhe.net
{
    public class MessageManager : MonoBehaviour
    {
        public static MessageManager messageManager;
        // Use this for initialization
        void Start()
        {
            messageManager = this;
        }
        /// <summary>
        /// 待处理的回调消息
        /// </summary>
        List<Message> callBackMessages = new List<Message>();
        /// <summary>
        /// 已发送的带回调，未处理回调的消息
        /// </summary>
        List<Message> sendMessages = new List<Message>();
        // Update is called once per frame
        void Update()
        {
            handler();
        }
        void handler()
        {
            lock (callBackMessages)
            {
                Debug.Log("当前消息条数:"+callBackMessages.Count);
                for (int i = 0; i < callBackMessages.Count; i++)
                {
                    Debug.Log(callBackMessages[i].protoBuffer.GetType());
                    getSendMessage(callBackMessages[i]);
                    callBackMessages[i].handle();
                    callBackMessages.RemoveAt(i);
                }
            }
        }
        public void addNewCallBackMessage(Message message)
        {
            lock (callBackMessages)
            {
                callBackMessages.Add(message);
            }
        }
        public void addNewSendMessage(Message message)
        {
            lock (sendMessages)
            {
                sendMessages.Add(message);
                MySocket.GetInstance().SendMessage(message);
            }
        }
        /// <summary>
        /// 匹配消息
        /// </summary>
        /// <returns></returns>
        public void getSendMessage(Message c)
        {
            lock (sendMessages)
            {
                for (int i = 0; i < sendMessages.Count; i++)
                {
                    if (c.cmd == sendMessages[i].cmd && c.cmd_branch == sendMessages[i].cmd_branch)
                    {
                        if (sendMessages[i].eventHandle != null)
                        {
                            c.eventHandle = sendMessages[i].eventHandle;
                            sendMessages.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
