using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using SocketGameProtocol;
using UnityEngine;


public class GameFace : MonoBehaviour
{
    private static GameFace instance;
    public static GameFace Instance => instance;
    private ClientManager clientManager;
    private RequestManager requestManager;
    public UIManager uiManager;
    public PlayerManager playerManager;
    public string UserName { get; set; }

    private void Awake()
    {
        instance = this;
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        uiManager = new UIManager(this);
        playerManager = new PlayerManager(this);
        uiManager.Init();
        clientManager.Init();
        requestManager.Init();
        playerManager.Init();
    }
    

    private void OnDestroy()
    {
        uiManager.OnDestroy();
        clientManager.OnDestroy();
        requestManager.OnDestroy();
    }

    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void HandleResponse(MainPack pack)
    {
        Debug.Log("GameFace处理响应");
        requestManager.HandleResponse(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestManager.RemoveRequest(actionCode);
    }
    /// <summary>
    /// 显示提示面板
    /// </summary>
    /// <param name="str"></param>
    public void ShowMessage(string str,bool isSync=false)
    {
        uiManager.ShowMessage(str,isSync);
    }
    
    /// <summary>
    /// 添加玩家
    /// </summary>
    /// <param name="playerPacks"></param>
    public void AddPlayer(MainPack pack)
    {
        playerManager.AddPlayer(pack);
    }
    /// <summary>
    /// 删除玩家
    /// </summary>
    /// <param name="id"></param>
    public void RemovePlayer(string id)
    {
        playerManager.RemovePlayer(id);
    }
    /// <summary>
    /// 游戏退出
    /// </summary>
    public void GameExit()
    {
        playerManager.GameExit();
        uiManager.PopPanel();
        uiManager.PopPanel();
    }
    /// <summary>
    /// 更新玩家位置
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePos(MainPack pack)
    {
        playerManager.UpdatePos(pack);
    }

    public void SpawnBullet(MainPack pack)
    {
        playerManager.SpawnBullet(pack);
    }
}
