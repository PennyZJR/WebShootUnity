using System;
using System.Collections.Generic;
/// <summary>
/// 泛型对象池（线程不安全版本）
/// 用于管理可重用对象的分配和回收，减少重复创建开销
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T>
{
    //存储空闲对象
    private readonly Stack<T> stack;
    
    //===== 对象池生命周期回调 =====
    /// <summary> 当需要创建新实例时调用的回调 </summary>
    public CustomDelegate.CallbackT<T> OnInstance{get;set;}
    /// <summary>
    /// 从池中获取对象时调用的回调（对象被激活前）
    /// </summary>
    public CustomDelegate.CallbackT<T> OnGet{get;set;}
    /// <summary>
    /// 对象释放回池时调用的回调（对象失活后）
    /// </summary>
    public CustomDelegate.CallbackT<T> OnRelease{get;set;}
    /// <summary>
    /// 池被清理时销毁对象调用的回调
    /// </summary>
    public CustomDelegate.CallbackT<T> OnDestroy{get;set;}
    
    //===== 池状态统计 =====
    /// <summary>
    /// 池创建的所有对象总数（含激活和未激活）
    /// </summary>
    public int CountAll { get; private set; }
    /// <summary>
    /// 当前池中空闲对象数量
    /// </summary>
    public int CountInactive=>stack.Count;
    /// <summary>
    /// 当前正在使用的对象数量
    /// </summary>
    public int CountActive=>CountAll-CountInactive;
    /// <summary>
    /// 构造对象池（带回调初始化）
    /// </summary>
    /// <param name="count">预分配容量</param>
    /// <param name="onInstance">实例化回调</param>
    /// <param name="onGet">获取对象回调</param>
    /// <param name="onRelease">释放对象回调</param>
    public ObjectPool(int count, CustomDelegate.CallbackT<T> onInstance, CustomDelegate.CallbackT<T> onGet, CustomDelegate.CallbackT<T> onRelease)
    {
        stack = new Stack<T>(count);
        OnInstance = onInstance;
        OnGet = onGet;
        OnRelease = onRelease;
    }
    /// <summary>
    /// 构造对象池（无回调版本）
    /// </summary>
    /// <param name="count"></param>
    public ObjectPool(int count)
    {
        stack = new Stack<T>(count);
    }
    /// <summary>
    /// 从池中获取对象实例
    /// </summary>
    /// <returns>可用的对象实例</returns>
    public T Get()
    {
        T val;
        //池为空时创建新实例
        if (stack.Count == 0)
        {
            val = OnInstance();//通过回调创建新对象
            CountAll++;//总计数增加
        }
        else
        {
            val=stack.Pop();//从栈顶取出对象
            //对象被意外销毁时的安全处理
            if (val == null)
            {
                val = OnInstance();//重新创建
                CountAll++;
            }
        }
        //触发获取回调（用于对象激活/初始化）
        OnGet?.Invoke();
        return val;
    }
    /// <summary>
    /// 将对象释放回池中
    /// </summary>
    /// <param name="element">要释放的对象</param>
    public void Release(T element)
    {
        //防止重复释放的安全检查
        if (stack.Count > 0 && (object)stack.Peek() == (object)element)
        {
            Console.WriteLine("Internal error.Trying to destroy object that is already released to pool");
            return;
        }
        //触发释放回调（用于对象清理/重置状态）
        OnRelease?.Invoke();
        //对象压回栈中
        stack.Push(element);
    }
    /// <summary>
    /// 清空对象池（销毁所有空闲对象）
    /// </summary>
    public void Clear()
    {
        while (stack.Count > 0)
        {
            stack.Pop();
            //触发销毁回调（用于资源释放）
            OnDestroy?.Invoke();
        }
    }
}