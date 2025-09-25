
using SocketGameProtocol;

public class CreateRoomRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            switch (pack.ReturnCode)
            {
                case ReturnCode.Success:
                    GameFace.Instance.uiManager.ShowMessage("创建房间成功");
                    GameFace.Instance.uiManager.PushPanel(UIPanelType.Room,UIWindowType.Middle);
                    GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).UpdatePlayerList(pack);
                    break;
                case ReturnCode.Fail:
                    GameFace.Instance.uiManager.ShowMessage("创建房间失败");
                    break;
            }
        });
    }

    public void SendRequest(string roomName,int maxNum)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        RoomPack room = new RoomPack();
        room.RoomName = roomName;
        room.MaxPlayerNum = maxNum;
        pack.RoomPacks.Add(room);
        pack.Str = "CreateRoom";
        base.SendRequest(pack);
    }
}
