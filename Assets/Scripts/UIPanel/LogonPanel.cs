using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 注册账号面板
/// </summary>
public class LogonPanel : BasePanel
{
    public LogonRequest logonRequest;

    public InputField userNameInput;

    public InputField passwordInput;
    public Button logonButton;
    public Button switchButton;

    private void Start()
    {
        logonButton.onClick.AddListener(OnLogonClick);
        switchButton.onClick.AddListener(OnSwitchClick);
    }

    private void OnSwitchClick()
    {
        GameFace.Instance.uiManager.PopPanel();
    }

    private void OnLogonClick()
    {
        if(userNameInput.text==null||userNameInput.text==null)
        {
            Debug.Log("用户名或密码不能为空");
            return;
        }
        logonRequest.SendRequest(userNameInput.text,passwordInput.text);
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
}
