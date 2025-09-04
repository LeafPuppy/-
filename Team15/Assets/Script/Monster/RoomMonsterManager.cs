using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMonsterManager : MonoBehaviour
{
    [SerializeField] Transform monstersRoot;
    [SerializeField] RewardUI rewardUI;
    int alive;

    void Start()
    {
        var monsters = monstersRoot.GetComponentsInChildren<MonsterCondition>(true);
        alive = 0;

        foreach (var m in monsters)
        {
            if (m == null) continue;
            alive++;
            m.OnDie += HandleDie;
        }
    }

    void OnDestroy()
    {
        if (monstersRoot == null) return;
        foreach (var m in monstersRoot.GetComponentsInChildren<MonsterCondition>(true))
            if (m != null) m.OnDie -= HandleDie;
    }

    void HandleDie(MonsterCondition _)
    {
        alive--;
        if (alive <= 0)
            rewardUI.Show();
    }
}

