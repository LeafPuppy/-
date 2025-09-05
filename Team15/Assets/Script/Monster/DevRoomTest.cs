using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DevRoomTest : MonoBehaviour
{
    [Header("Hotkeys")]
    public KeyCode killAllKey = KeyCode.F6;
    public KeyCode openRewardKey = KeyCode.F7;
    public KeyCode finishRewardKey = KeyCode.F8;

    void Update()
    {
        if (Input.GetKeyDown(killAllKey)) StartCoroutine(KillAllFlow());
        if (Input.GetKeyDown(openRewardKey)) OpenReward();
        if (Input.GetKeyDown(finishRewardKey)) FinishReward();
    }

    IEnumerator KillAllFlow()
    {
        var room = DungeonMapManager.Instance?.CurrentMap;
        if (!room) { Debug.LogWarning("[DEV] No current room"); yield break; }

        int total = 0, invoked = 0, disabled = 0;

        foreach (var m in room.GetComponentsInChildren<MonsterCondition>(true))
        {
            if (m == null) continue; total++;
            var t = m.GetType();

            // 1) TakeDamage(999999...) 시도
            var takeDmg = t.GetMethod("TakeDamage",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (takeDmg != null)
            {
                var ps = takeDmg.GetParameters();
                object[] args = BuildArgs(ps, 999999f);
                takeDmg.Invoke(m, args);
                invoked++;
                continue;
            }

            // 2) Kill/Die 계열 메서드 시도
            var kill = t.GetMethod("Kill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? t.GetMethod("ForceKill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var die = t.GetMethod("Die", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    ?? t.GetMethod("ForceDie", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var method = kill ?? die;
            if (method != null) { method.Invoke(m, null); invoked++; continue; }

            // 3) 최후: 비활성(이 경우 OnDie는 안 뜰 수 있음)
            m.gameObject.SetActive(false);
            disabled++;
        }

        Debug.Log($"[DEV] total={total}, invoked={invoked}, disabled={disabled}");

        // 한 프레임 기다려 사망 처리/이벤트 전파 시간 확보
        yield return null;

        // 아직 보상 UI가 안 떴으면 RoomMonsterManager에 직접 신호
        var rmm = room.GetComponentInChildren<RoomMonsterManager>(true);
        if (rmm != null)
        {
            var mi = typeof(RoomMonsterManager).GetMethod("ShowIfNotOpened",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (mi != null) { mi.Invoke(rmm, null); }
            else
            {
                // 최후: RewardUI 직접 오픈
                var reward = FindObjectOfType<RewardUI>(true);
                reward?.Show();
            }
        }
    }

    object[] BuildArgs(System.Reflection.ParameterInfo[] ps, float damage)
    {
        var args = new object[ps.Length];
        for (int i = 0; i < ps.Length; i++)
        {
            var p = ps[i];
            if (i == 0 && p.ParameterType == typeof(float)) { args[i] = damage; continue; }
            args[i] = p.HasDefaultValue ? p.DefaultValue :
                      (p.ParameterType.IsValueType ? System.Activator.CreateInstance(p.ParameterType) : null);
        }
        return args;
    }

    void OpenReward()
    {
        var reward = FindObjectOfType<RewardUI>(true);
        if (!reward) { Debug.LogWarning("[DEV] RewardUI not found"); return; }

        var mapUI = MapSelectUI.Instance;
        var type = mapUI ? mapUI.GetCurrentNodeTypeSafe() : MapType.Normal;
        var hasNext = mapUI ? mapUI.HasNextFromCurrent() : true;

        reward.Configure(type, hasNext);
        reward.Show();
        Debug.Log("[DEV] RewardUI.Show() opened manually.");
    }

    void FinishReward()
    {
        var reward = FindObjectOfType<RewardUI>(true);
        if (!reward) { Debug.LogWarning("[DEV] RewardUI not found"); return; }
        reward.Close(); // 선택 시뮬레이션 → 바인더가 지도 열어줌
        Debug.Log("[DEV] RewardUI.Close() called (simulate pick).");
    }
}
