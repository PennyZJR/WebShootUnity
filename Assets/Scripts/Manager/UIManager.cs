using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager
{
    private Dictionary<UIPanelType, BasePanel> panelDic = new Dictionary<UIPanelType, BasePanel>();
    private Dictionary<UIPanelType, string> panelPathDic = new Dictionary<UIPanelType, string>();
    private Dictionary<UIWindowType, Transform> windowType2Transform = new Dictionary<UIWindowType, Transform>();
    /// <summary>
    /// 存储当前正在显示的UI面板
    /// </summary>
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private Transform canvas;
    private Transform bottom;
    private Transform middle;
    private Transform top;
    private MessagePanel messagePanel;
    public UIManager(GameFace face) : base(face)
    {
    }

    public override void Init()
    {
        base.Init();
        InitPanel();
        canvas = GameObject.Find("Canvas").transform;
        bottom = canvas.Find("Bottom");
        middle = canvas.Find("Middle");
        top = canvas.Find("Top");
        windowType2Transform.Add(UIWindowType.Bottom, bottom);
        windowType2Transform.Add(UIWindowType.Middle, middle);
        windowType2Transform.Add(UIWindowType.Top, top);
        PushPanel(UIPanelType.Message, UIWindowType.Top);
        PushPanel(UIPanelType.Start, UIWindowType.Bottom);
    }
    /// <summary>
    /// 把UI显示在界面上
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="windowType">窗口层级</param>
    public void PushPanel(UIPanelType panelType,UIWindowType windowType)
    {
        if (panelDic.TryGetValue(panelType, out BasePanel panel))
        {
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            if(!panelStack.Contains(panel))
                panelStack.Push(panel);
            panel.OnEnter();
        }
        else
        {
            if (panelStack.Count > 0)
            {
                panelStack.Peek().OnPause();
            }
            BasePanel newPanel=SpawnPanel(panelType, windowType);
            panelStack.Push(newPanel);
            newPanel.OnEnter();
        }

    }
    /// <summary>
    /// 关闭当前UI
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count == 0) return;
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();
        if (panelStack.Count > 0)
        {
            BasePanel panel = panelStack.Peek();
            panel.OnRecovery();
        }
    }
    /// <summary>
    /// 实例化对应UI
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel SpawnPanel(UIPanelType panelType,UIWindowType windowType)
    {

        if (panelPathDic.TryGetValue(panelType, out string path))
        {
            GameObject obj = Resources.Load<GameObject>(path);
            GameObject ins=GameObject.Instantiate(obj, windowType2Transform[windowType]);
            BasePanel basePanel = ins.GetComponent<BasePanel>();
            panelDic.Add(panelType, basePanel);
            return basePanel;
        }
        else
        {
            return null;
        }
        
    }
    /// <summary>
    /// 初始化UI面板预设体路径
    /// </summary>
    private void InitPanel()
    {
        string panelPath = "UIPanel/";
        string[] paths = new string[]
            { "MessagePanel", "StartPanel", "LogonPanel", "LoginPanel", "GamePanel", "GameOverPanel", "RoomListPanel" ,"RoomPanel"};
        panelPathDic.Add(UIPanelType.Message,panelPath+paths[0]);
        panelPathDic.Add(UIPanelType.Start, panelPath + paths[1]);
        panelPathDic.Add(UIPanelType.Logon, panelPath + paths[2]);
        panelPathDic.Add(UIPanelType.Login, panelPath + paths[3]);
        panelPathDic.Add(UIPanelType.Game, panelPath + paths[4]);
        panelPathDic.Add(UIPanelType.GameOver, panelPath + paths[5]);
        panelPathDic.Add(UIPanelType.RoomList, panelPath + paths[6]);
        panelPathDic.Add(UIPanelType.Room, panelPath + paths[7]);
    }

    public void SetMessagePanel(MessagePanel panel)
    {
        messagePanel = panel;
    }
    public void ShowMessage(string str,bool isSync=false)
    {
        messagePanel.ShowMessage(str,isSync);
    }

    public T GetPanel<T>(UIPanelType panelType) where T : BasePanel
    {
        if (panelDic.TryGetValue(panelType, out BasePanel panel))
            return panel as T;
        return null;
    }
}
