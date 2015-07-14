using UnityEngine;
using System.Collections;
using th.nx;
/// <summary>
/// 窗口管理
/// </summary>
namespace com.tianhe
{
    public class WindowManager : MonoBehaviour
    {
        WindowManager manager;
        /// <summary>
        /// 画布
        /// </summary>
        GameObject canvas;
        // Use this for initialization
        void Start()
        {
            //Log.debug("ENTER WindowManager.Start");
            manager = this;
            canvas = GameObject.Find("Canvas");
            loadPrefabs("Loginwindow");
            //Instantiate(Resources.Load("prefabs/scene"));
            //Log.debug("LEAVE WindowManager.Start");
        }
        /// <summary>
        /// 登录窗口
        /// </summary>
        [HideInInspector]
        public LoginWindow loginWindow;

        public void loadPrefabs(string name)
        {
            GameObject obj = Instantiate(Resources.Load("prefabs/"+name)) as GameObject;
            obj.transform.SetParent(canvas.transform, true);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            if (name.Equals("Loginwindow"))
            {
                loginWindow = obj.GetComponent<LoginWindow>();
            }
        }
    }
}