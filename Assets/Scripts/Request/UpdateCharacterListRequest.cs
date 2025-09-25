
using SocketGameProtocol;
/// <summary>
/// 针对非房主玩家退出
/// </summary>
public class UpdateCharacterListRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode=ActionCode.UpdateCharacterList;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.uiManager.GetPanel<GamePanel>(UIPanelType.Game).UpdateList(pack);
            GameFace.Instance.RemovePlayer(pack.Str);
            GameFace.Instance.uiManager.GetPanel<RoomListPanel>(UIPanelType.RoomList).UpdateRoomList(pack);
        });
    }
    
}
