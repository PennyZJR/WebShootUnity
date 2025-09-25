
using SocketGameProtocol;

public class StartingRequest:BaseRequest
{
    public override void Awake()
    {
        actionCode = ActionCode.Starting;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.ShowMessage("游戏正式开始！");
            //创建角色
            GameFace.Instance.playerManager.AddPlayer(pack);
            GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).GameStart(pack);
        });
    }
}
