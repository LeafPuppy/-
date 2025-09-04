using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RunOutcome { None, Cleared, Died, Quit }
public enum StarterWeaponKind { None, Sword, Bow, Staff }

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [Header("�б�")]
    public bool hasMetMerchant;          // ���� ���� �� ���ΰ� ù ���� ��ȭ �ߴ°�
    public bool everEnteredDungeon;      // ������ �� ���̶� ���°�
    public bool inDungeon;               // ���� ���� ���ΰ�

    [Header("���� �� �б�")]
    public int totalRunsCompleted;       // ������ �ǵ��ƿ� �÷��� Ƚ��(Ŭ����/����/����)
    public RunOutcome lastRunOutcome = RunOutcome.None;
    public int lastHandledRunIndex;      // '���� ��ȯ �� ù ��ȭ

    [Header("���� ���� ����")]
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

    // ������ ���ƿ��� ��(���� �� �ε� �� ȣ��)
    public void OnReturnToVillage()
    {
        inDungeon = false;
        if (lastRunOutcome != RunOutcome.None)
            totalRunsCompleted++;
    }
}
