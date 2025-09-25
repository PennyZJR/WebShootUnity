
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem:MonoBehaviour
{
    public Button btnJoin;
    public Text txtName;
    public Text txtNumber;
    public Text txtStatus;

    private void Start()
    {
        btnJoin.onClick.AddListener(OnJoinBtnClick);
    }
    /// <summary>
    /// 设置显示信息
    /// </summary>
    /// <param name="name"></param>
    /// <param name="num"></param>
    /// <param name="status"></param>
    public void SetRoomInfo(string name,int nowNum,int maxNum,int status)
    {
        txtName.text = name;
        txtNumber.text = nowNum + "/" + maxNum;
        if (status == 0)
            txtStatus.text = "等待加入";
        else if (status == 1)
            txtStatus.text = "房间已满";
        else if (status == 2)
            txtStatus.text = "游戏中";
    }
    private void OnJoinBtnClick()
    {
        //发送加入房间请求
        GameFace.Instance.uiManager.GetPanel<RoomListPanel>(UIPanelType.RoomList).JoinRoom(txtName.text);
    }
}
