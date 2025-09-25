
using SocketGameProtocol;

public class PlayersRequest:BaseRequest
{
    public override void Awake()
    {
        //不需要requestCode,客户端只获取这个数据，不发
        actionCode = ActionCode.PlayerList;
        base.Awake();
    }

    
    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).UpdatePlayerList(pack);
        });
        base.OnResponse(pack);
    }
}
