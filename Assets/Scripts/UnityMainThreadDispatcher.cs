using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Unity主线程调度器，用于在非主线程中执行Unity相关操作
/// </summary>
public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher _instance;
    private readonly Queue<Action> _executionQueue = new Queue<Action>();
    public static UnityMainThreadDispatcher Instance => _instance;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }
}
