
using SocketGameProtocol;

public class JoinRoomRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<RoomListPanel>(UIPanelType.RoomList).JoinRoomResponse(pack);
        });
        base.OnResponse(pack);
    }
    
    public void SendRequest(string roomName)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = roomName;
        base.SendRequest(pack);
    }
}
