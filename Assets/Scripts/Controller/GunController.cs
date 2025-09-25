using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController:MonoBehaviour
{
    private GameObject bullet; // 子弹预制体
    private Transform firePoint; // 开火位置与朝向

    private FireRequest fireRequest;
    // 开火与对象池配置
    [Header("Fire Settings")]
    [SerializeField] private float fireRate = 0.15f; // 连射间隔（秒）
    [SerializeField] private float bulletSpeed = 20f; // 子弹初速度（需要子弹上有Rigidbody2D）
    [SerializeField] private float bulletLifeTime = 3f; // 子弹存活时长（秒）

    [Header("Pool Settings")]
    [SerializeField] private int prewarmCount = 20; // 预热数量

    private float nextFireTime = 0f;
    // 使用提供的泛型对象池
    private ObjectPool<GameObject> _bulletPool;
    private Transform poolRoot; // 池容器，便于层级管理

    private SpriteRenderer sr;

    private void Start()
    {
        sr=GetComponent<SpriteRenderer>();
        fireRequest = gameObject.AddComponent<FireRequest>();
        bullet = Resources.Load<GameObject>("Prefabs/bullet");
        firePoint = transform.Find("Fire").transform;
        // 初始化对象池
        CreatePoolRoot();
        InitPool();
        PrewarmPool();
    }

    private void Update()
    {
        if (Application.isFocused)
        {
            LookAt();
            Fire();
        }
    }

    private void LookAt()
    {
        Vector3 dir=Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if(transform.eulerAngles.z>=90&&transform.eulerAngles.z<=270)
        {
            sr.flipY = true;
        }
        else
        {
            sr.flipY = false;
        }
    }

    private void Fire()
    {
        // 输入校验与射速限制
        if (Time.time < nextFireTime) return;
        if (bullet == null || firePoint == null) return;
        if (Input.GetMouseButtonDown(0))
        {
            nextFireTime = Time.time + fireRate;

            // 从对象池获取子弹
            GameObject b = _bulletPool.Get();
            b.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            b.SetActive(true);

            // 如有刚体，赋初速度（2D场景使用right方向）
            if (b.TryGetComponent<Rigidbody2D>(out var rb2d))
            {
                rb2d.velocity = firePoint.right * bulletSpeed;
            }
            fireRequest.SendRequest(firePoint.position,firePoint.eulerAngles.z,bulletSpeed);
        }
    }

    // —— 使用你提供的对象池 ——
    private void CreatePoolRoot()
    {
        if (poolRoot != null) return;
        var root = new GameObject("BulletPool");
        poolRoot = root.transform;
        poolRoot.SetParent(null);
    }

    private void InitPool()
    {
        // 仅设置容量，回调用属性赋值
        _bulletPool = new ObjectPool<GameObject>(prewarmCount)
        {
            // 创建实例：生成到 poolRoot 下，并默认失活
            OnInstance = () =>
            {
                var go = Instantiate(bullet, poolRoot);
                go.SetActive(false);
                Bullet b = go.GetComponent<Bullet>();
                b.SetLiveTime(bulletLifeTime);
                b.pool = _bulletPool;
                return go;
            },
            OnGet = null,
            OnRelease = null,
            OnDestroy = null
        };
    }

    private void PrewarmPool()
    {
        if (prewarmCount <= 0 || _bulletPool == null) return;
        // 通过 Get+Release 方式预热对象
        for (int i = 0; i < prewarmCount; i++)
        {
            var obj = _bulletPool.OnInstance();
            if(obj.activeSelf)obj.SetActive(false);
            _bulletPool.Release(obj);
        }
    }
    
}
