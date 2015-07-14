using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using com.tianhe.account.module.account.model;
using Assets.com.tianhe.account.module.account.model;
using com.tianhe.net;
using System;
/// <summary>
/// 登录界面
/// </summary>
public class LoginWindow : MonoBehaviour
{
    /// <summary>
    /// 登录按钮
    /// </summary>
    public Button loginButton;
    /// <summary>
    /// 帐号
    /// </summary>
    public InputField account;
    /// <summary>
    /// 密码
    /// </summary>
    public InputField password;
    /// <summary>
    /// 提示信息
    /// </summary>
    public Text tips;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void submit()
    {
        tips.text = "";
        string accountText = account.text;
        string passwordText = password.text;
        if (string.IsNullOrEmpty(accountText))
        {
            tips.text = "用户名不能为空！";
            return;
        }
        if (string.IsNullOrEmpty(passwordText))
        {
            tips.text = "密码不能为空！";
            return;
        }
        LoginVo vo = new LoginVo(accountText,passwordText,3,getTime(),"");
        com.tianhe.net.Message message = new com.tianhe.net.Message(GlobalConst.LOGIN, GlobalConst.LOGIN_LOGIN, vo, (object o) => {
            LoginResult result = (LoginResult)o;
            callback(result);
        });
        MessageManager.messageManager.addNewSendMessage(message);
    }

    public long getTime()
    {
        DateTime geo = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime cur = DateTime.Now;
        TimeSpan span = cur.Subtract(geo);
        long time = (long)span.TotalMilliseconds;
        return time;
    }

    public void callback(LoginResult result)
    {
        if (result.code == 0)
        {
            tips.text = "登录成功！";
            Instantiate(Resources.Load("prefabs/Scene"));
            Destroy(gameObject);
        }
        else
        {
            tips.text = "登录失败！" + result.code;
        }
    }
}
