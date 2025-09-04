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

    [Header("Cards (버튼 2장 형태)")]
    [SerializeField] Button cardLeft;
    [SerializeField] Button cardRight;
    [SerializeField] TMP_Text leftText;
    [SerializeField] TMP_Text rightText;

    [Header("Accessory Mode (악세사리 선택 시 3개로 전환)")]
    [SerializeField] Button cardCenter;
    [SerializeField] TMP_Text centerText;
    [SerializeField] Image leftIcon;
    [SerializeField] Image rightIcon;
    [SerializeField] Image centerIcon;

    [Header("Title / Guide")]
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text guideText;

    //[Header("Optional: 보상 종료 후 열 지도 UI")]
    //[SerializeField] GameObject mapSelectUI;
    //[SerializeField] bool openMapOnClose = false;

    public event Action OnFinished;

    bool isOpen = false;
    bool inputLocked = false;

    bool accessoryMode = false;
    List<AccessorySO> accOptions = new();
    int picksRemaining = 0;
    Action afterAccessory;

    void Awake()
    {
        if (panel) panel.SetActive(false);
    }

    void OnDisable()
    {
        if (isOpen && Time.timeScale == 0f) Time.timeScale = 1f;
        isOpen = false;
        inputLocked = false;
        ExitAccessoryMode();
    }

    public void Configure(MapType mapType, bool nextStage)
    {
        roomType = mapType;
        hasNextStage = nextStage;
    }

    public void Show() => ShowInternal(roomType, hasNextStage);
    public void Show(MapType mapType, bool nextStage = true) => ShowInternal(mapType, nextStage);

    void ShowInternal(MapType mapType, bool nextStage)
    {
        if (!panel) return;

        ResetCards();
        ExitAccessoryMode();

        panel.SetActive(true);
        isOpen = true;
        inputLocked = false;
        Time.timeScale = 0f;

        var diff = GameState.Instance ? GameState.Instance.currentDifficulty : Difficulty.Normal;
        bool isHard = diff == Difficulty.Hard;

        switch (mapType)
        {
            case MapType.Normal:
                if (titleText) titleText.text = isHard ? "일반 맵 보상 (어려움)" : "일반 맵 보상";

                SetCard(cardLeft, leftText, leftIcon, "랜덤 악세사리 1개", () =>
                {
                    var pick = AccessoryManager.Instance?.DrawOneRandomAllowDupExceptNonStackableOwned();
                    if (pick != null) AccessoryManager.Instance.Equip(pick);
                    Finish();
                });

                SetCard(cardRight, rightText, rightIcon, isHard ? "체력 회복 (10%)" : "체력 회복 (20%)", () =>
                {
                    Heal(isHard ? 0.10f : 0.20f);
                    Finish();
                });
                break;

            case MapType.Special:
                if (titleText) titleText.text = isHard ? "특수 맵 보상 (어려움)" : "특수 맵 보상";

                if (!isHard)
                {
                    SetCard(cardLeft, leftText, leftIcon, "악세사리 3개 중 1개 선택", () =>
                    {
                        EnterAccessoryMode(showCount: 3, pickCount: 1, onDone: Finish);
                    });

                    SetCard(cardRight, rightText, rightIcon, "체력 회복 (40%)", () =>
                    {
                        Heal(0.40f);
                        Finish();
                    });
                }
                else
                {
                    SetCard(cardLeft, leftText, leftIcon, "랜덤 악세사리 1개", () =>
                    {
                        var pick = AccessoryManager.Instance?.DrawOneRandomAllowDupExceptNonStackableOwned();
                        if (pick != null) AccessoryManager.Instance.Equip(pick);
                        Finish();
                    });

                    SetCard(cardRight, rightText, rightIcon, "체력 회복 (20%)", () =>
                    {
                        Heal(0.20f);
                        Finish();
                    });
                }
                break;

            case MapType.Boss:
                if (titleText) titleText.text = isHard ? "보스 맵 보상 (어려움)" : "보스 맵 보상";

                SafeSetActive(cardRight, false);

                string desc = isHard
                    ? "보석 10개 + (다음 스테이지 있으면) 악세 3중1 + 체력 회복 30%"
                    : "보석 5개  + (다음 스테이지 있으면) 악세 3중2 + 체력 회복 60%";

                SetCard(cardLeft, leftText, leftIcon, desc, () =>
                {
                    GiveGems(isHard ? 10 : 5);

                    if (nextStage)
                    {
                        int picks = isHard ? 1 : 2;
                        EnterAccessoryMode(
                            showCount: 3,
                            pickCount: picks,
                            onDone: () =>
                            {
                                Heal(isHard ? 0.30f : 0.60f);
                                Finish();
                            });
                    }
                    else
                    {
                        Heal(isHard ? 0.30f : 0.60f);
                        Finish();
                    }
                });
                break;
        }
    }

    void EnterAccessoryMode(int showCount, int pickCount, Action onDone)
    {
        accessoryMode = true;
        afterAccessory = onDone;

        accOptions = AccessoryManager.Instance?.DrawOptions(Mathf.Clamp(showCount, 1, 3))
                     ?? new List<AccessorySO>();
        picksRemaining = Mathf.Clamp(pickCount, 1, accOptions.Count);

        // 3장 모드로 전환
        RemoveAll(cardLeft);
        RemoveAll(cardRight);
        RemoveAll(cardCenter);

        if (titleText) titleText.text = "악세사리 선택";
        UpdateGuide();

        SetupAccCard(0, cardLeft, leftText, leftIcon);
        SetupAccCard(1, cardRight, rightText, rightIcon);
        SetupAccCard(2, cardCenter, centerText, centerIcon);
    }

    void ExitAccessoryMode()
    {
        accessoryMode = false;
        afterAccessory = null;
        accOptions.Clear();
        picksRemaining = 0;

        SetIcon(leftIcon, null, false);
        SetIcon(rightIcon, null, false);
        SetIcon(centerIcon, null, false);

        SafeSetActive(cardCenter, false);
    }

    void SetupAccCard(int idx, Button btn, TMP_Text label, Image icon)
    {
        bool active = idx < accOptions.Count;
        SafeSetActive(btn, active);
        if (!active) return;

        var acc = accOptions[idx];
        if (label) label.text = acc ? acc.displayName : "-";
        SetIcon(icon, acc ? acc.icon : null, acc && acc.icon != null);

        btn.onClick.AddListener(() => PickAccessory(idx));
    }

    void PickAccessory(int index)
    {
        if (!accessoryMode) return;
        if (index >= accOptions.Count) return;

        var pick = accOptions[index];
        AccessoryManager.Instance?.Equip(pick);
        picksRemaining--;

        // 선택된 카드는 제거 → 중복 선택 방지
        accOptions.RemoveAt(index);

        if (picksRemaining <= 0)
        {
            var cb = afterAccessory;
            ExitAccessoryMode();
            cb?.Invoke();
            return;
        }

        RefreshAccCards();
    }

    void RefreshAccCards()
    {
        RemoveAll(cardLeft);
        RemoveAll(cardRight);
        RemoveAll(cardCenter);

        SetupAccCard(0, cardLeft, leftText, leftIcon);
        SetupAccCard(1, cardRight, rightText, rightIcon);
        SetupAccCard(2, cardCenter, centerText, centerIcon);

        UpdateGuide();
    }

    void UpdateGuide()
    {
        if (guideText) guideText.text = $"남은 선택: {picksRemaining}";
    }

    void ResetCards()
    {
        RemoveAll(cardLeft);
        RemoveAll(cardRight);
        RemoveAll(cardCenter);

        SafeSetActive(cardLeft, false);
        SafeSetActive(cardRight, false);
        SafeSetActive(cardCenter, false);

        SetIcon(leftIcon, null, false);
        SetIcon(rightIcon, null, false);
        SetIcon(centerIcon, null, false);
    }

    void SetCard(Button btn, TMP_Text label, Image icon, string text, Action onPick)
    {
        if (!btn || !label) return;

        btn.interactable = true;
        SafeSetActive(btn, true);
        label.text = text;
        SetIcon(icon, null, false); // 기본 모드에선 텍스트만

        btn.onClick.AddListener(() =>
        {
            if (inputLocked) return;
            inputLocked = true;

            if (cardLeft) cardLeft.interactable = false;
            if (cardRight) cardRight.interactable = false;
            if (cardCenter) cardCenter.interactable = false;

            onPick?.Invoke();
        });
    }

    void SetCard(Button btn, TMP_Text label, string text, Action onPick)
        => SetCard(btn, label, null, text, onPick);

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        inputLocked = false;

        panel.SetActive(false);
        Time.timeScale = 1f;

        // if (openMapOnClose && mapSelectUI) mapSelectUI.SetActive(true);

        OnFinished?.Invoke();
    }

    void Finish() => Close();

    void Heal(float percent)
    {
        var p = CharacterManager.Instance?.Player;
        if (p == null) return;

        float add = p.maxHealth * percent;
        p.currentHealth = Mathf.Min(p.maxHealth, p.currentHealth + add);
    }

    void GiveGems(int count)
    {
        Debug.Log($"[Reward] 보석 {count}개");
    }

    void SafeSetActive(Button b, bool v) { if (b) b.gameObject.SetActive(v); }
    void SafeSetActive(GameObject go, bool v) { if (go) go.SetActive(v); }
    void RemoveAll(Button b) { if (b) b.onClick.RemoveAllListeners(); }
    void SetIcon(Image img, Sprite spr, bool enable)
    {
        if (!img) return;
        img.sprite = spr;
        img.enabled = enable && spr != null;
    }
}
