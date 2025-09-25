
using System;

public static class GameEvent
{
    public static event Action<string, int> OnHpChanged;//id,hp
    public static event Action<string> OnPlayerDead;//id
    public static void RaiseHpChanged(string id,int hp)=>OnHpChanged?.Invoke(id,hp);
    public static void RaisePlayerDead(string id)=>OnPlayerDead?.Invoke(id);
}
