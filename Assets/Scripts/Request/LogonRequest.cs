using SocketGameProtocol;
using UnityEngine;

/// <summary>
/// 注册请求
/// </summary>
public class LogonRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Logon;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            switch (pack.ReturnCode)
            {
                case ReturnCode.Success:
                    Debug.Log("注册成功");
                    GameFace.Instance.ShowMessage("注册成功");
                    GameFace.Instance.uiManager.PushPanel(UIPanelType.Login, UIWindowType.Middle);
                    break;
                case ReturnCode.Fail:
                    Debug.Log("注册失败");
                    GameFace.Instance.ShowMessage("注册失败");
                    break;
            }
        });
    }

    public void SendRequest(string userName,string password)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        LoginPack loginPack=new LoginPack();
        loginPack.UserName = userName;
        loginPack.Password = password;
        pack.LoginPack = loginPack;
        base.SendRequest(pack);
    }
}
