
using SocketGameProtocol;

public class StartGameRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode=ActionCode.StartGame;    
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "StartGame";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).StartGameResponse(pack);
        });
    }
}
