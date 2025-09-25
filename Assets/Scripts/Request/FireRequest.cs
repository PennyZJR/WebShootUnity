
using SocketGameProtocol;
using UnityEngine;

public class FireRequest:BaseRequest
{
    public override void Awake()
    {
        requestCode=RequestCode.Game;
        actionCode=ActionCode.Fire;
        base.Awake();
    }

    public void SendRequest(Vector2 pos, float rot,float bulletSpeed)
    {
        MainPack pack = new MainPack();
        BulletPack bulletPack = new BulletPack();
        bulletPack.PosX = pos.x;
        bulletPack.PosY = pos.y;
        bulletPack.BulletSpeed = bulletSpeed;
        bulletPack.RotZ = rot;
        pack.BulletPack = bulletPack;
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            GameFace.Instance.SpawnBullet(pack);
        });
    }
}
