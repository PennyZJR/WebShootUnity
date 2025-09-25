
using System;
using System.Net;
using System.Net.Sockets;
using SocketGameProtocol;
using UnityEngine;

public class ClientManager:BaseManager
{
    private Socket socket;
    private Message message;
    public ClientManager(GameFace face) : base(face)
    {
    }

    public override void Init()
    {
        base.Init();
        message = new Message();
        InitSocket(8080);
    }
    /// <summary>
    /// 初始化Socket
    /// </summary>
    /// <param name="port"></param>
    private void InitSocket(int port)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.Connect(IPAddress.Parse("127.0.0.1"), port);
            StartReceive();
            GameFace.Instance.ShowMessage("连接服务器成功");
        }
        catch (SocketException e)
        {
            Debug.Log($"连接服务器失败"+e.Message);
            GameFace.Instance.ShowMessage("连接服务器失败");
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        Close();
    }
    /// <summary>
    /// 关闭Socket
    /// </summary>
    private void Close()
    {
        if (socket != null && socket.Connected)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }
    }
    /// <summary>
    /// 接收消息
    /// </summary>
    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.CurrentIndex, message.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            if (socket == null || !socket.Connected)
                return;
            int len = socket.EndReceive(ar);
            if (len == 0)
            {
                Close();
                return;
            }
            message.ReadBuffer(len,HandleResponse);
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.Log("接受消息出错" + e.Message);
        }
    }

    private void HandleResponse(MainPack pack)
    {
        gameFace.HandleResponse(pack);
        Debug.Log("ClientManager处理消息");
    }

    public void Send(MainPack pack)
    {
        socket.Send(Message.PackData(pack));
    }
}
