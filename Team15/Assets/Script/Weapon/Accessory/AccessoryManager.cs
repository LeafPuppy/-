using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryManager : MonoBehaviour
{
    public static AccessoryManager Instance { get; private set; }

    [Header("모든 악세사리 풀 (SO)")]
    public List<AccessorySO> allAccessories = new();

    [Header("획득/장착 목록")]
    public List<AccessorySO> equipped = new();

    public static int GlobalBonusDamage = 0;   // 짱돌: +1
    public static float GlobalAttackDelta = 0f;  // 커피: -0.2f
    public static float GlobalSizeScale = 1f;  // 돋보기: x1.5

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<AccessorySO> DrawOptions(int count)
    {
        var result = new List<AccessorySO>();
        if (allAccessories == null || allAccessories.Count == 0) return result;

        var candidates = new List<AccessorySO>();
        foreach (var a in allAccessories)
            if (a) candidates.Add(a);

        int need = Mathf.Min(count, candidates.Count);
        bool bootsUsed = false;

        while (result.Count < need && candidates.Count > 0)
        {
            int idx = Random.Range(0, candidates.Count);
            var pick = candidates[idx];

            if (pick.type == AccessoryType.Boots && bootsUsed)
            {
                candidates.RemoveAt(idx);
                continue;
            }

            result.Add(pick);
            if (pick.type == AccessoryType.Boots) bootsUsed = true;

            candidates.RemoveAt(idx);
        }

        return result;
    }

    public void Equip(AccessorySO acc)
    {
        if (!acc) return;

        if (!acc.stackable && equipped.Exists(a => a && a.type == acc.type))
            return;

        equipped.Add(acc);
        Apply(acc);
    }

    public void EquipMany(IEnumerable<AccessorySO> list)
    {
        foreach (var a in list) Equip(a);
    }

    void Apply(AccessorySO acc)
    {
        var player = CharacterManager.Instance?.Player;
        var pc = player ? player.controller : null;

        switch (acc.type)
        {
            case AccessoryType.Boots:
                // 공중 점프 +1
                pc?.AddExtraAirJumps(1);
                break;

            case AccessoryType.RubberBand:
                // 이속 +0.25
                pc?.AddMoveSpeed(0.25f);
                break;

            case AccessoryType.SecondHeart:
                // 대쉬 쿨타임 50%로
                pc?.MultiplyDashCooldown(0.5f);
                break;

            case AccessoryType.Stone:
                GlobalBonusDamage += 1;
                break;

            case AccessoryType.Coffee:
                GlobalAttackDelta -= 0.2f;
                break;

            case AccessoryType.Magnifier:
                GlobalSizeScale *= 1.5f;
                break;
        }
    }
}