
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel:BasePanel
{
    public LoginRequest loginRequest;

    public InputField userNameInput;

    public InputField passwordInput;
    public Button loginButton;
    public Button switchButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginClick);
        switchButton.onClick.AddListener(OnSwitchClick);
    }

    private void OnSwitchClick()
    {
        GameFace.Instance.uiManager.PushPanel(UIPanelType.Logon,UIWindowType.Middle);
    }

    private void OnLoginClick()
    {
        if(userNameInput.text==null||userNameInput.text==null)
        {
            Debug.Log("用户名或密码不能为空");
            return;
        }
        loginRequest.SendRequest(userNameInput.text,passwordInput.text);
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
