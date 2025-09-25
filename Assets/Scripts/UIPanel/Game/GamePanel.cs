
using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel:BasePanel
{
    [SerializeField]
    private GameObject playerInfoItem;
    [SerializeField]
    private Transform listTransform;
    [SerializeField]
    private Text txtTime;
    [SerializeField]
    private Button btnExit;
    private Dictionary<string,PlayerInfoItem>itemDic=new Dictionary<string,PlayerInfoItem>();
    private float startTime;
    public GameExitRequest gameExitRequest;
    public UpdateCharacterListRequest updateCharacterListRequest;
    private void Start()
    {
        startTime = Time.time;
        btnExit.onClick.AddListener(OnBtnExitClick);
    }

    private void FixedUpdate()
    {
        txtTime.text = Math.Clamp((int)(Time.time - startTime), 0, 300)+ " S";
    }
    public void UpdateList(MainPack pack)
    {
        for (int i = 0; i < listTransform.childCount; i++)
        {
            GameObject.Destroy(listTransform.GetChild(i).gameObject);
        }
        itemDic.Clear();

        foreach (var playerPack in pack.PlayerPacks)
        {

            GameObject go = Instantiate(playerInfoItem, listTransform);
            PlayerInfoItem pInfo = go.GetComponent<PlayerInfoItem>();
            pInfo.Set(playerPack.PlayerName, playerPack.PlayerHp);
            itemDic.Add(playerPack.PlayerName, pInfo);
            
        }
    }

    public void UpdateHp(string id,int value)
    {
        if(itemDic.TryGetValue(id,out var item))
        {
            item.UpdateHp(value);
        }
        else
        {
            Debug.Log("获取不到对应的角色信息");
        }
    }
    private void OnBtnExitClick()
    {
        gameExitRequest.SendRequest();
        GameFace.Instance.GameExit();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
        GameEvent.OnHpChanged += HandleHpChanged;
        GameEvent.OnPlayerDead += HandlePlayerDead;
    }

    private void HandlePlayerDead(string obj)
    {
        gameExitRequest.SendRequest();
        GameFace.Instance.GameExit();
    }

    private void HandleHpChanged(string id, int hp)
    {
        UpdateHp(id,hp);
    }

    public override void OnExit()
    {
        base.OnExit();
        Exit();
        GameEvent.OnHpChanged -= HandleHpChanged;
        GameEvent.OnPlayerDead -= HandlePlayerDead;
    }

    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }

    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    public void Enter()
    {
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }
}
