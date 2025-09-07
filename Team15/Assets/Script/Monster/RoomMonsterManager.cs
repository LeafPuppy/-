using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMonsterManager : MonoBehaviour
{
    [Header("전멸 감시 대상 루트")]
    [SerializeField] Transform monstersRoot;

    [Header("보상 UI 연결")]
    [SerializeField] RewardUI rewardUI;

    [Header("현재 방 정보(보상 분기용)")]
    [SerializeField] MapType roomType = MapType.Normal;
    [SerializeField] bool hasNextStage = true;

    [Header("연출 딜레이")]
    [SerializeField] float rewardOpenDelay = 0.0f;

    int alive;
    bool opened;

    void Awake()
    {
        if (!rewardUI) rewardUI = FindObjectOfType<RewardUI>(true);
    }

    void Start()
    {
        if (!monstersRoot)
        {
            var map = DungeonMapManager.Instance ? DungeonMapManager.Instance.CurrentMap : null;
            if (map)
            {
                monstersRoot = map.transform.Find("Monster")
                             ?? map.transform.Find("Monsters");
            }
        }

        if (MapSelectUI.Instance != null)
        {
            roomType = MapSelectUI.Instance.GetCurrentNodeTypeSafe();
            hasNextStage = MapSelectUI.Instance.HasNextFromCurrent();
        }

        SetupSubscriptions();

        if (alive <= 0)
            OpenRewardSafe();

        StartCoroutine(FallbackWatch());
    }

    void OnDestroy()
    {
        if (monstersRoot == null) return;
        foreach (var m in monstersRoot.GetComponentsInChildren<MonsterCondition>(true))
            if (m != null) m.OnDie -= HandleDie;
    }

    void SetupSubscriptions()
    {
        alive = 0;

        if (!monstersRoot) return;

        var monsters = monstersRoot.GetComponentsInChildren<MonsterCondition>(true);
        foreach (var m in monsters)
        {
            if (m == null) continue;
            alive++;
            m.OnDie += HandleDie;
        }
    }

    void HandleDie(MonsterCondition _)
    {
        alive--;
        if (alive <= 0)
            StartCoroutine(OpenRewardAfterDelayRealtime());
    }

    void OpenRewardSafe()
    {
        if (opened) return;
        opened = true;

        if (!rewardUI)
        {
            if (UIManager.Instance != null)
                rewardUI = UIManager.Instance.Show<RewardUI>();
            else
                rewardUI = FindObjectOfType<RewardUI>(true);
        }

        if (!rewardUI)
        {
            Debug.LogWarning("[RoomMonsterManager] RewardUI를 찾을 수 없어 열지 못했습니다.");
            return;
        }

        rewardUI.Configure(roomType, hasNextStage);
        rewardUI.Show();
    }

    IEnumerator OpenRewardAfterDelayRealtime()
    {
        if (rewardOpenDelay > 0f)
            yield return new WaitForSecondsRealtime(rewardOpenDelay);
        else
            yield return null;

        OpenRewardSafe();
    }

    IEnumerator FallbackWatch()
    {
        while (!opened)
        {
            yield return new WaitForSecondsRealtime(0.25f);

            if (!monstersRoot)
            {
                OpenRewardSafe();
                yield break;
            }

            int activeCount = 0;
            var monsters = monstersRoot.GetComponentsInChildren<MonsterCondition>(true);
            foreach (var m in monsters)
            {
                if (!m) continue;
                if (m.gameObject.activeInHierarchy) activeCount++;
            }

            if (activeCount == 0)
            {
                OpenRewardSafe();
                yield break;
            }
        }
    }

    void ShowIfNotOpened()
    {
        if (opened) return;
        OpenRewardSafe();
    }
}

