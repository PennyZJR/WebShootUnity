
using SocketGameProtocol;

public class ChatRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Chat;
        base.Awake();
    }

    public void SendRequest(string str)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = str;
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).ChatResponse(pack.Str);
        });
    }
}
