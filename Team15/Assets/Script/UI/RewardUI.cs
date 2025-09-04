using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RewardUI : MonoBehaviour
{
    [Header("Room Type (현재 방 타입)")]
    [SerializeField] MapType roomType = MapType.Normal;   // 인스펙터에서 설정
    [SerializeField] bool hasNextStage = true;            // 마지막 방이면 false로

    [Header("Root")]
    [SerializeField] GameObject panel;

    [Header("Cards (버튼 3장 형태)")]
    [SerializeField] Button cardLeft;
    [SerializeField] Button cardRight;
    [SerializeField] Button cardCenter;

    [Header("Labels")]
    [SerializeField] TMP_Text leftText;
    [SerializeField] TMP_Text rightText;
    [SerializeField] TMP_Text centerText;
    [SerializeField] TMP_Text titleText;

    [Header("Accessory Select UI 참조")]
    [SerializeField] AccessorySelectUI accessorySelectUI;

    public event Action OnFinished;

    void Awake()
    {
        if (panel) panel.SetActive(false);
    }

    public void Show() => ShowInternal(roomType, hasNextStage);
    public void Show(MapType mapType, bool nextStage = true) => ShowInternal(mapType, nextStage);

    void ShowInternal(MapType mapType, bool nextStage)
    {
        ResetCards();

        var diff = GameState.Instance ? GameState.Instance.currentDifficulty : Difficulty.Normal;
        bool isHard = diff == Difficulty.Hard;

        switch (mapType)
        {
            case MapType.Normal:
                if (titleText) titleText.text = isHard ? "일반 맵 보상 (어려움)" : "일반 맵 보상";

                SetCard(cardLeft, leftText, "랜덤 악세사리 1개", () =>
                {
                    var opts = AccessoryManager.Instance?.DrawOptions(1);
                    if (opts != null && opts.Count > 0)
                        AccessoryManager.Instance.Equip(opts[0]);

                    Close();
                });

                SetCard(cardRight, rightText, isHard ? "체력 회복 (10%)" : "체력 회복 (20%)", () =>
                {
                    Heal(isHard ? 0.10f : 0.20f);
                    Close();
                });

                cardCenter.gameObject.SetActive(false);
                break;

            case MapType.Special:
                if (titleText) titleText.text = isHard ? "특수 맵 보상 (어려움)" : "특수 맵 보상";

                if (!isHard)
                {
                    SetCard(cardLeft, leftText, "악세사리 3개 중 1개 선택", () =>
                    {
                        RequestAccessorySelection(3, 1, () => { Close(); });
                    });
                    SetCard(cardRight, rightText, "체력 회복 (40%)", () =>
                    {
                        Heal(0.40f);
                        Close();
                    });
                }
                else
                {
                    SetCard(cardLeft, leftText, "랜덤 악세사리 1개", () =>
                    {
                        var opts = AccessoryManager.Instance?.DrawOptions(1);
                        if (opts != null && opts.Count > 0)
                            AccessoryManager.Instance.Equip(opts[0]);
                        Close();
                    });
                    SetCard(cardRight, rightText, "체력 회복 (20%)", () =>
                    {
                        Heal(0.20f);
                        Close();
                    });
                }

                cardCenter.gameObject.SetActive(false);
                break;

            case MapType.Boss:
                if (titleText) titleText.text = isHard ? "보스 맵 보상 (어려움)" : "보스 맵 보상";

                cardLeft.gameObject.SetActive(false);
                cardRight.gameObject.SetActive(false);

                string desc = isHard
                    ? "보석 10개 + (다음 스테이지 있으면) 악세 3중1 + 체력 회복 30%"
                    : "보석 5개  + (다음 스테이지 있으면) 악세 3중2 + 체력 회복 60%";

                SetCard(cardCenter, centerText, desc, () =>
                {
                    GiveGems(isHard ? 10 : 5);

                    if (nextStage)
                    {
                        int picks = isHard ? 1 : 2;
                        RequestAccessorySelection(3, picks, () =>
                        {
                            Heal(isHard ? 0.30f : 0.60f);
                            Close();
                        });
                    }

                    Heal(isHard ? 0.30f : 0.60f);

                    Close();
                });
                break;
        }

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    void ResetCards()
    {
        cardLeft.onClick.RemoveAllListeners();
        cardRight.onClick.RemoveAllListeners();
        cardCenter.onClick.RemoveAllListeners();

        cardLeft.gameObject.SetActive(false);
        cardRight.gameObject.SetActive(false);
        cardCenter.gameObject.SetActive(false);
    }

    void SetCard(Button btn, TMP_Text label, string text, Action onPick)
    {
        if (!btn || !label) return;
        btn.gameObject.SetActive(true);
        label.text = text;
        btn.onClick.AddListener(() => onPick?.Invoke());
    }

    public void Close()
    {
        if (panel) panel.SetActive(false);
        Time.timeScale = 1f;
        OnFinished?.Invoke();
    }

    void Heal(float percent)
    {
        var p = CharacterManager.Instance?.Player;
        if (!p) return;
        float add = p.maxHealth * percent;
        p.currentHealth = Mathf.Min(p.maxHealth, p.currentHealth + add);
    }

    void GiveGems(int count)
    {
        Debug.Log($"[Reward] 보석 {count}개");
    }

    void RequestAccessorySelection(int showCount, int pickCount, Action onDone)
    {
        if (!accessorySelectUI)
        {
            onDone?.Invoke();
            return;
        }

        if (panel) panel.SetActive(false);

        accessorySelectUI.Open(showCount, pickCount, () =>
        {
            if (panel) panel.SetActive(true);
            onDone?.Invoke();
        });
    }
}
