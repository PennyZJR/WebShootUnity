
using System;
using System.Collections;
using UnityEngine;

public class Bullet:MonoBehaviour
{
    private float bulletLiveTime;
    private Rigidbody2D rb;
    /// <summary>
    /// 子弹所属于的对象池
    /// </summary>
    public ObjectPool<GameObject> pool;
    private Coroutine lifeCoroutine;
    private int damage = 10;
    [SerializeField]private LayerMask hitLayers;//仅检测玩家层
    [SerializeField]private string ownerId;
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if(lifeCoroutine!=null)
            StopCoroutine(lifeCoroutine);
        lifeCoroutine = StartCoroutine(AutoReleaseAfter(bulletLiveTime));
    }
    public void SetOwnerId(string id)
    {
        ownerId = id;
    }

    private IEnumerator AutoReleaseAfter(float t)
    {
        yield return new WaitForSeconds(t);
        ReleaseToPool();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //过滤层
        if (((1 << other.gameObject.layer) & hitLayers) == 0)
        {
            ReleaseToPool();
            return;
        }
        if(other.TryGetComponent<Health>(out var health))//不打自己
        {
            if (!string.IsNullOrEmpty(ownerId) && health.name == ownerId)
            {
                ReleaseToPool();
                return;
            }
            health.TakeDamage(damage,ownerId);
        }
        ReleaseToPool();
    }

    public void SetLiveTime(float time)
    {
        bulletLiveTime = time;
    }

    private void ReleaseToPool()
    {
        if(rb!=null)
            rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
        pool?.Release(gameObject);
    }

    private void OnDisable()
    {
        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }
}
