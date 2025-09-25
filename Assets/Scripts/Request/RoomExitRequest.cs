
using SocketGameProtocol;

public class RoomExitRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Exit;
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "Exit";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).ExitRoomResponse();
            GameFace.Instance.uiManager.GetPanel<RoomListPanel>(UIPanelType.RoomList).UpdateRoomList(pack);
        });
    }
}
