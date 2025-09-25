
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class RequestManager:BaseManager
{
    public RequestManager(GameFace face) : base(face)
    {
    }

    private Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(BaseRequest request)
    {
        requestDic.TryAdd(request.GetActionCode, request);
    }
    public void RemoveRequest(ActionCode actioncode)
    {
        requestDic.Remove(actioncode);
    }

    public void HandleResponse(MainPack pack)
    {
        if (requestDic.TryGetValue(pack.ActionCode, out BaseRequest request))
        {
            request.OnResponse(pack);
            Debug.Log("requestManager处理响应");
        }
        else
        {
            Debug.LogWarning("找不到对应的处理方法"+pack.ActionCode);
        }
    }
}
