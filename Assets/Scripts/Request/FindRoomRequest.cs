
using System;
using SocketGameProtocol;

public class FindRoomRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.FindRoom;
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "FindRoom";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            switch (pack.ReturnCode)
            {
                case ReturnCode.Success:
                    GameFace.Instance.uiManager.ShowMessage("查询成功！一共有"+pack.RoomPacks.Count+"个房间");
                    break;
                case ReturnCode.Fail:
                    GameFace.Instance.uiManager.ShowMessage("查询出错");
                    break;
                case ReturnCode.NoRoom:
                    GameFace.Instance.uiManager.ShowMessage("当前没有房间");
                    break;
            }
            var panel = GameFace.Instance.uiManager.GetPanel<RoomListPanel>(UIPanelType.RoomList);
            panel?.UpdateRoomList(pack);
        });
    }
}
