
using SocketGameProtocol;
using Unity.VisualScripting;
using UnityEngine;

public class UpdatePosRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode=ActionCode.UpdatePos;
        base.Awake();
    }

    public void SendRequest(Vector2 pos,float characterRot,float gunRot,bool flipY)
    {
        MainPack pack = new MainPack();
        PosPack posPack = new PosPack();
        PlayerPack playerPack = new PlayerPack();
        posPack.PosX = pos.x;
        posPack.PosY = pos.y;
        posPack.RotZ = characterRot;
        posPack.GunRotZ = gunRot;
        playerPack.PlayerName = GameFace.Instance.UserName;
        playerPack.PosPack = posPack;
        playerPack.FlipY = flipY;
        pack.PlayerPacks.Add(playerPack);
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.UpdatePos(pack);
        });
    }
}
