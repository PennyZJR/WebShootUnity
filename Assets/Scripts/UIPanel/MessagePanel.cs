
using System;
using UnityEngine.UI;

public class MessagePanel:BasePanel
{
    public Text text;
    private string msg = null;
    public override void OnEnter()
    {
        base.OnEnter();
        text.CrossFadeAlpha(0,0.1f,false);
        GameFace.Instance.uiManager.SetMessagePanel(this);
    }

    private void Update()
    {
        if (msg != null)
        {
            ShowText(msg);
            msg = null;
        }
    }

    private void ShowText(string str)
    {
        text.text = str;
        text.CrossFadeAlpha(1,0.1f,false);
        Invoke("HideText",1);
    }
    /// <summary>
    /// 异步不能直接访问Unity中的相关资源
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isSync"></param>
    public void ShowMessage(string str,bool isSync=false)
    {
        if (isSync)
        {
            msg = str;
        }
        else
        {
            ShowText(str);
        }
    }

    public void HideText()
    {
        text.CrossFadeAlpha(0,1f,false);
    }
}
