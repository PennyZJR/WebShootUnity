
using System;
using Google.Protobuf.Collections;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel:BasePanel
{
    public Button btnQutiRoom;
    public Button btnSend;
    public Button btnStart;
    public Text chatText;
    public InputField inputText;
    public Scrollbar scrollbar;
    public Transform content;
    public GameObject userItem;
    public PlayersRequest playersRequest;
    public RoomExitRequest roomExitRequest;
    public ChatRequest chatRequest;
    public StartGameRequest startGameRequest;
    public StartingRequest startingRequest;
    private void Start()
    {
        btnQutiRoom.onClick.AddListener(OnQuitRoomClick);
        btnSend.onClick.AddListener(OnSendClick);
        btnStart.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }

    private void OnSendClick()
    {
        if(inputText.text=="")
            GameFace.Instance.uiManager.ShowMessage("发送内容不能为空");
        else
        {
            chatRequest.SendRequest(inputText.text);
            chatText.text+="我："+inputText.text+"\n";
            inputText.text="";
        }
    }
    
    private void OnQuitRoomClick()
    {
        roomExitRequest.SendRequest();
        //GameFace.Instance.uiManager.PopPanel();
    }
    /// <summary>
    /// 刷新玩家列表
    /// </summary>
    /// <param name="pack"></param>
    public void UpdatePlayerList(MainPack pack)
    {
        for(int i=content.childCount-1;i>=0;i--)
            Destroy(content.GetChild(i).gameObject);
        foreach (PlayerPack player in pack.PlayerPacks)
        {
            UserItem item = GameObject.Instantiate(userItem, content).GetComponent<UserItem>();
            item.SetData(player.PlayerName);
        }
    }

    public void ExitRoomResponse()
    {
        GameFace.Instance.uiManager.PopPanel();
    }

    public void StartGameResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                GameFace.Instance.uiManager.ShowMessage("游戏已启动");
                break;
            case ReturnCode.Fail:
                GameFace.Instance.uiManager.ShowMessage("开始游戏失败！您不是房主");
                break;
        }
    }

    public void GameStart(MainPack pack)
    {
        GameFace.Instance.uiManager.PushPanel(UIPanelType.Game, UIWindowType.Middle);
        GamePanel gamePanel = GameFace.Instance.uiManager.GetPanel<GamePanel>(UIPanelType.Game);
        gamePanel.UpdateList(pack);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }

    public override void OnExit()
    {
        base.OnExit();
        Exit();
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

    public void ChatResponse(string packStr)
    {
        chatText.text += packStr+"\n";
    }
}
