
using SocketGameProtocol;
/// <summary>
/// 只针对房主退出
/// </summary>
public class GameExitRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.ExitGame;
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack= new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.Str = "GameExit";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.GameExit();
        });
    }
}
