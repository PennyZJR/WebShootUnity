
using System;
using SocketGameProtocol;
using UnityEngine;

public class BaseRequest:MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    public ActionCode GetActionCode => actionCode;

    public virtual void Awake()
    {
        
    }

    private void Start()
    {
        GameFace.Instance.AddRequest(this);
    }

    public virtual void OnDestroy()
    {
        GameFace.Instance.RemoveRequest(actionCode);
    }
    /// <summary>
    /// 接收方法
    /// </summary>
    /// <param name="pack"></param>
    public virtual void OnResponse(MainPack pack)
    {
        
    }
    /// <summary>
    /// 发送方法
    /// </summary>
    /// <param name="pack"></param>
    public virtual void SendRequest(MainPack pack)
    {
        GameFace.Instance.Send(pack);
    }
}
