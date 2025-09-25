
using SocketGameProtocol;

public class LoginRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode=ActionCode.Login;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            switch (pack.ReturnCode)
            {
                case ReturnCode.Success:
                    GameFace.Instance.ShowMessage("登录成功");
                    GameFace.Instance.uiManager.PushPanel(UIPanelType.RoomList, UIWindowType.Middle);
                    GameFace.Instance.UserName=pack.LoginPack.UserName;
                    break;
                case ReturnCode.Fail:
                    GameFace.Instance.ShowMessage("登录失败");
                    break;
            }
        });
    }

    public void SendRequest(string userName, string password)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        LoginPack loginPack = new LoginPack();
        loginPack.UserName = userName;
        loginPack.Password = password;
        pack.LoginPack = loginPack;
        base.SendRequest(pack);
    }
}
