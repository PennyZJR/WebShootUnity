using System.Collections.Generic;
using Google.Protobuf.Collections;
using SocketGameProtocol;
using UnityEngine;

/// <summary>
/// 管理一场游戏中的所有玩家
/// </summary>
public class PlayerManager:BaseManager
{
    /// <summary>
    /// 存储所有玩家，key为player的唯一id,用playerName代替
    /// </summary>
    Dictionary<string,GameObject>players=new Dictionary<string, GameObject>();
    /// <summary>
    /// 玩家角色预制体
    /// </summary>
    private GameObject charactrer;
    /// <summary>
    /// 创造初始位置
    /// </summary>
    private Transform spawnPos;
    /// <summary>
    /// 子弹预设体
    /// </summary>
    private GameObject bullet;
    
    //--对象池相关
    private ObjectPool<GameObject> bulletPool;
    private Transform poolRoot;//池容器
    private int prewarmCount=20;//预热数量
    private float bulletLifeTime = 3f;//子弹存活时长
    public PlayerManager(GameFace face) : base(face)
    {
    }

    public override void Init()
    {
        base.Init();
        charactrer=Resources.Load<GameObject>("Prefabs/Character");
        bullet=Resources.Load<GameObject>("Prefabs/bullet");
        //初始化对象池
        CreatePoolRoot();
        InitPool();
        PrewarmPool();
    }

    private void PrewarmPool()
    {
        if (prewarmCount <= 0 || bulletPool == null) return;
        // 通过 Get+Release 方式预热对象
        for (int i = 0; i < prewarmCount; i++)
        {
            var obj = bulletPool.OnInstance();
            if(obj.activeSelf)obj.SetActive(false);
            bulletPool.Release(obj);
        }
    }

    private void InitPool()
    {
        bulletPool = new ObjectPool<GameObject>(prewarmCount)
        {
            OnInstance = () =>
            {
                var go = Object.Instantiate(bullet, poolRoot);
                go.SetActive(false);
                Bullet b = go.GetComponent<Bullet>();
                if (b != null)
                {
                    b.SetLiveTime(bulletLifeTime);
                    b.pool = bulletPool;
                }

                return go;
            }
        };
    }

    private void CreatePoolRoot()
    {
        if (poolRoot != null) return;
        var root = new GameObject("RemoteBulletPool");
        poolRoot=root.transform;
        Object.DontDestroyOnLoad(root);
    }

    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="playerPack"></param>
    public void AddPlayer(MainPack pack)
    {
        spawnPos = GameObject.Find("SpawnPos").transform;
        foreach (var p in pack.PlayerPacks)
        {
            // 在出生点基础上增加一个随机偏移
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector3 spawnPosition = spawnPos.position + new Vector3(randomOffset.x, 0, 0);
            var go=GameObject.Instantiate(charactrer, spawnPosition, Quaternion.identity);
            if (p.PlayerName.Equals(GameFace.Instance.UserName))
            {
                //创建本地角色,添加控制脚本
                go.AddComponent<PlayerController>();
                go.transform.Find("player/HandGun").gameObject.AddComponent<GunController>();
                go.AddComponent<UpdatePosRequest>();
                go.AddComponent<UpdatePos>();
                //在GunController添加了
                //go.AddComponent<FireRequest>();
            }
            else
            {
                go.GetComponent<Rigidbody2D>().simulated = false;
            }
            //创建其他客户端的角色
            go.GetComponent<Health>().SetPlayerId(p.PlayerName);
            players.Add(p.PlayerName,go);
        }
    }
    /// <summary>
    /// 得到场景中所有玩家信息
    /// </summary>
    /// <returns></returns>
    public RepeatedField<PlayerPack> GetAllPlayerInfo()
    {
        RepeatedField<PlayerPack>packs = new RepeatedField<PlayerPack>();
        foreach ( var kv in players)
        {
            PlayerPack pack = new PlayerPack();
            pack.PlayerName = kv.Key;
            pack.PlayerHp = kv.Value.GetComponent<Health>().CurrentHp;
            packs.Add(pack);
        }

        return packs;
    }
    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="id"></param>
    public void RemovePlayer(string id)
    {
        if (players.TryGetValue(id, out GameObject go))
        {
            GameObject.Destroy(go);
            players.Remove(id);
        }
        else
        {
            Debug.Log("移除角色出错");
        }
    }

    public void GameExit()
    {
        foreach (GameObject player in players.Values)
        {
            GameObject.Destroy(player);
        }
        players.Clear();
        if (poolRoot != null)
        {
            GameObject.Destroy(poolRoot.gameObject);
            bulletPool?.Clear();
        }
    }

    public void UpdatePos(MainPack pack)
    {
        PosPack posPack = pack.PlayerPacks[0].PosPack;
        if (players.TryGetValue(pack.PlayerPacks[0].PlayerName, out GameObject go))
        {
            Vector2 pos = new Vector2(posPack.PosX,posPack.PosY);
            float characterRot = posPack.RotZ;
            float gunRot=posPack.RotZ;
            bool flipY=pack.PlayerPacks[0].FlipY;
            go.transform.position = pos;
            //go.transform.rotation=Quaternion.Euler(0,0,characterRot);
            Transform hangGunTransform=go.transform.Find("player/HandGun");
            hangGunTransform.rotation = Quaternion.Euler(0,0,gunRot);
            hangGunTransform.gameObject.GetComponent<SpriteRenderer>().flipY=flipY;
        }
    }

    public void SpawnBullet(MainPack pack)
    {
        Vector3 pos = new Vector3(pack.BulletPack.PosX, pack.BulletPack.PosY, 0);
        float rot =pack.BulletPack.RotZ;
        GameObject go = bulletPool.Get();
        go.transform.position = pos;
        go.transform.rotation=Quaternion.Euler(0,0,rot);
        go.SetActive(true);
        Rigidbody2D rb=go.GetComponent<Rigidbody2D>();
        go.GetComponent<Bullet>().SetOwnerId(GameFace.Instance.UserName);
        rb.velocity=go.transform.right*pack.BulletPack.BulletSpeed;
    }

    public GameObject GetPlayerObject(string id)
    {
        players.TryGetValue(id, out var value);
        return value!=null ? value : null;

    }
}
