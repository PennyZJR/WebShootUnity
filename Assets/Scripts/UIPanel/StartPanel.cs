
using System;
using UnityEngine.UI;

public class StartPanel:BasePanel
{
    public Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnStartClick);
    }

    private void OnStartClick()
    {
        GameFace.Instance.uiManager.PushPanel(UIPanelType.Login, UIWindowType.Middle);
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

    private void Enter()
    {
        gameObject.SetActive(true);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
