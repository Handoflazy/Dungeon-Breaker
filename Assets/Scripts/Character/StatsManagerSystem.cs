using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatsManagerSystem : PlayerSystem
{
    public void AddXp(int xp)
    {
        print(xp + "duoc trao");
        if (player.playerStats == null) return;
        player.playerStats.xp += xp;
        player.playerStats.Upgrade(OnUpgradeStats);
        player.ID.playerEvents.OnAddXp?.Invoke();
        player.playerStats.Save();
    }

    private void OnUpgradeStats()
    {
        player.ID.playerEvents.OnLevelUp?.Invoke();
    }
}
