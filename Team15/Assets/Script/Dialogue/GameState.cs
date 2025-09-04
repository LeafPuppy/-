using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RunOutcome { None, Cleared, Died, Quit }
public enum StarterWeaponKind { None, Sword, Bow, Staff }

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("분기")]
    public bool hasMetMerchant;          // 게임 시작 후 상인과 첫 만남 대화 했는가
    public bool everEnteredDungeon;      // 던전에 한 번이라도 들어갔는가
    public bool inDungeon;               // 현재 던전 안인가

    [Header("실행 중 분기")]
    public int totalRunsCompleted;       // 마을로 되돌아온 플레이 횟수(클리어/죽음/포기)
    public RunOutcome lastRunOutcome = RunOutcome.None;
    public int lastHandledRunIndex;      // '마을 귀환 후 첫 대화

    [Header("무기 선택 상태")]
    public StarterWeaponKind currentStarterWeapon = StarterWeaponKind.None;
    [HideInInspector] public StarterWeaponKind pendingStarterWeapon = StarterWeaponKind.None;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnTalkedToMerchantFirstTime() { hasMetMerchant = true; }

    public void OnDungeonEnter()
    {
        inDungeon = true;
        everEnteredDungeon = true;
        lastRunOutcome = RunOutcome.None;
    }

    public void OnDungeonCleared() { lastRunOutcome = RunOutcome.Cleared; }
    public void OnPlayerDied() { lastRunOutcome = RunOutcome.Died; }
    public void OnPlayerQuitRun() { lastRunOutcome = RunOutcome.Quit; }

    // 마을로 돌아왔을 때(마을 씬 로드 시 호출)
    public void OnReturnToVillage()
    {
        inDungeon = false;
        if (lastRunOutcome != RunOutcome.None)
            totalRunsCompleted++;
    }
}
