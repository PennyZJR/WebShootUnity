
public class BaseManager
{
    protected GameFace gameFace;

    public BaseManager(GameFace face)
    {
        gameFace = face;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init()
    {
        
    }
    /// <summary>
    /// 销毁时调用的函数
    /// </summary>
    public virtual void OnDestroy()
    {
        
    }
}
