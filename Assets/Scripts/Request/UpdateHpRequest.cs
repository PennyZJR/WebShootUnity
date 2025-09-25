
using SocketGameProtocol;

public class UpdateHpRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.UpdateHp;
        base.Awake();
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        pack.PlayerPacks .AddRange(GameFace.Instance.playerManager.GetAllPlayerInfo());
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GamePanel gamePanel= GameFace.Instance.uiManager.GetPanel<GamePanel>(UIPanelType.Game);
            for (int i = 0; i < pack.PlayerPacks.Count; i++)
            {
                gamePanel.UpdateHp(pack.PlayerPacks[i].PlayerName,pack.PlayerPacks[i].PlayerHp);    
                GameFace.Instance.playerManager.GetPlayerObject(pack.PlayerPacks[i].PlayerName)?.GetComponent<Health>().ApplyServerHp(pack.PlayerPacks[i].PlayerHp);
            }
            
        });
    }
}
