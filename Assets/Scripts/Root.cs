using UnityEngine;
using System.Collections;
using com.tianhe.net;
using System.Threading;
using th.nx;
/// <summary>
/// 启动类
/// </summary>
namespace com.tianhe
{
    public class Root : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            //Log.debug("ENTER Root.Start");
            //gameObject.AddComponent<MessageManager>();
            //MySocket.GetInstance();
            //gameObject.AddComponent<WindowManager>();
            gameObject.AddComponent<AudioManagerMiniBattle>();
            //Log.debug("LEAVE Root.Start");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        void OnDestroy()
        {
            
        }
        void OnApplicationQuit()
        {
            //MySocket.GetInstance().Closed();
        }
    }
}