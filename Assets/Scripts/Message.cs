using System;
using System.Linq;
using Google.Protobuf;
using SocketGameProtocol;


public class Message
{
    private byte[] buffer = new byte[1024];

    private int currentIndex = 0;
    /// <summary>
    /// 缓冲数组
    /// </summary>
    public byte[] Buffer => buffer;
    /// <summary>
    /// 缓冲数组当前存储数据的长度
    /// </summary>
    public int CurrentIndex => currentIndex;
    /// <summary>
    /// 缓冲数组剩余空间
    /// </summary>
    public int RemainSize => buffer.Length - currentIndex;
    public void ReadBuffer(int len,Action<MainPack>handleRequest)
    {
        currentIndex += len;
        //小于等于包头长度，不进行解析
        if(currentIndex<=4)
        {
            return;
        }
        //消息体长度
        int count = BitConverter.ToInt32(buffer, 0);
        while(true)
        {
            if (currentIndex - 4 >= count)
            {
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                handleRequest?.Invoke(pack);
                Array.Copy(buffer, sizeof(int) + count, buffer, 0, currentIndex - (sizeof(int) + count));
                currentIndex -= (sizeof(int) + count);
                if (currentIndex > 4)
                {
                    count = BitConverter.ToInt32(buffer, 0);
                }
                else
                {
                    break;
                }
            }
            else break;
        }
    }
    /// <summary>
    /// 打包数据，加入包头（存的是包体的长度）
    /// </summary>
    /// <param name="pack"></param>
    /// <returns></returns>
    public static  byte[]PackData(MainPack pack)
    {
        byte[]data=pack.ToByteArray();
        byte[] head = BitConverter.GetBytes(data.Length);
        return head.Concat(data).ToArray();
    }
}
