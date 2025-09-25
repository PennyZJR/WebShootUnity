using System;
using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    public Button btnExit;
    public Button btnSearch;
    public Button btnCreate;
    public InputField roomNameInput;
    public Transform roomListTransform;
    public GameObject roomItem;
    public CreateRoomRequest createRoomRequest;
    public FindRoomRequest findRoomRequest;
    public JoinRoomRequest joinRoomRequest;
    public Slider slider;
    public Text txtNowNum;
    private void Start()
    {
        btnExit.onClick.AddListener(OnExitClick);
        btnSearch.onClick.AddListener(OnSearchClick);
        btnCreate.onClick.AddListener(OnCreateClick);
        slider.onValueChanged.AddListener((value) =>
        {
            txtNowNum.text = ((int)value).ToString();
        });
    }

    private void OnCreateClick()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            GameFace.Instance.ShowMessage("房间名不能为空");
            return;
        }

        createRoomRequest.SendRequest(roomNameInput.text,(int)slider.value);
    }
    
    private void OnSearchClick()
    {
        findRoomRequest.SendRequest();
    }

    private void OnExitClick()
    {
        GameFace.Instance.uiManager.PopPanel();
    }

    public void UpdateRoomList(MainPack pack)
    {
        //清除原来的房间
        for(int i=roomListTransform.childCount-1;i>=0;i--)
            Destroy(roomListTransform.GetChild(i).gameObject);
        foreach (RoomPack room in pack.RoomPacks)
        {
            RoomItem item = Instantiate(roomItem, roomListTransform).GetComponent<RoomItem>();
            item.SetRoomInfo(room.RoomName,room.NowPlayerNum,room.MaxPlayerNum,room.RoomStatus);
        }
    }

    public void JoinRoom(string roomName)
    {
        joinRoomRequest.SendRequest(roomName);
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

    public void JoinRoomResponse(MainPack pack)
    {
        switch (pack.ReturnCode)
        {
            case ReturnCode.Success:
                GameFace.Instance.uiManager.ShowMessage("加入房间成功");
                GameFace.Instance.uiManager.PushPanel(UIPanelType.Room,UIWindowType.Middle);
                GameFace.Instance.uiManager.GetPanel<RoomPanel>(UIPanelType.Room).UpdatePlayerList(pack);
                break;
            case ReturnCode.Fail:
                GameFace.Instance.uiManager.ShowMessage("加入房间失败");
                break;
            default:
                GameFace.Instance.uiManager.ShowMessage("房间不存在");
                break;
        }
    }
}
