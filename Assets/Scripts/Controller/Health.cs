
using System;
using Unity.VisualScripting;
using UnityEngine;
[DisallowMultipleComponent]
public class Health:MonoBehaviour
{
    [SerializeField] private string playerId;
    [SerializeField] private int maxHp=100;
    private UpdateHpRequest updateHpRequest;
    public int CurrentHp { get; private set; }

    private void Awake()
    {
        CurrentHp = maxHp;
        updateHpRequest = this.AddComponent<UpdateHpRequest>();
    }

    public void SetPlayerId(string id)
    {
        playerId = id;
    }
    public void Init(string id, int hp)
    {
        playerId = id;
        CurrentHp = Mathf.Clamp(hp, 0, maxHp);
        GameEvent.RaiseHpChanged(playerId, CurrentHp);
    }

    public void TakeDamage(int damage, string attackerId)
    {
        if (CurrentHp <= 0) return;
        CurrentHp = Mathf.Clamp(CurrentHp - Mathf.Max(0,damage), 0, maxHp);
        GameEvent.RaiseHpChanged(playerId, CurrentHp);
        if (CurrentHp <= 0)
        {
            GameEvent.RaisePlayerDead(playerId);
        }
        //TODO:发送更新血条请求
        updateHpRequest.SendRequest();
    }

    public void ApplyServerHp(int newHp)
    {
        CurrentHp=Mathf.Clamp(newHp, 0, maxHp);
        GameEvent.RaiseHpChanged(playerId, CurrentHp);
        if(CurrentHp<=0)GameEvent.RaisePlayerDead(playerId);
    }
}
