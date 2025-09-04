using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AccessorySelectUI : MonoBehaviour
{
    public static AccessorySelectUI Instance { get; private set; }

    [Header("Root")]
    [SerializeField] GameObject panel;

    [Header("카드(최대 3개)")]
    [SerializeField] Button btnA; [SerializeField] TMP_Text txtA; [SerializeField] Image imgA;
    [SerializeField] Button btnB; [SerializeField] TMP_Text txtB; [SerializeField] Image imgB;
    [SerializeField] Button btnC; [SerializeField] TMP_Text txtC; [SerializeField] Image imgC;

    [Header("Confirm")]
    [SerializeField] Button btnConfirm;
    [SerializeField] TMP_Text txtGuide;

    int _pickCount = 1;
    Action _onDone;
    List<AccessorySO> _options = new();
    HashSet<int> _picked = new();

    void Awake()
    {
        Instance = this;
        if (panel) panel.SetActive(false);

        btnA.onClick.AddListener(() => TogglePick(0));
        btnB.onClick.AddListener(() => TogglePick(1));
        btnC.onClick.AddListener(() => TogglePick(2));
        btnConfirm.onClick.AddListener(Confirm);
    }

    public void Open(int showCount, int pickCount, Action onDone)
    {
        _pickCount = Mathf.Clamp(pickCount, 1, 3);
        _onDone = onDone;
        _picked.Clear();

        _options = AccessoryManager.Instance?.DrawOptions(Mathf.Clamp(showCount, 1, 3))
                   ?? new List<AccessorySO>();

        SetCard(0, btnA, txtA, imgA);
        SetCard(1, btnB, txtB, imgB);
        SetCard(2, btnC, txtC, imgC);

        if (txtGuide) txtGuide.text = $"{_options.Count}개 중 {_pickCount}개 선택";
        btnConfirm.interactable = false;

        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    void SetCard(int i, Button btn, TMP_Text t, Image img)
    {
        bool active = i < _options.Count;
        btn.gameObject.SetActive(active);
        if (!active) return;

        var acc = _options[i];
        if (t) t.text = acc ? acc.displayName : "-";
        if (img) img.sprite = acc ? acc.icon : null;
    }

    void TogglePick(int index)
    {
        if (index >= _options.Count) return;

        if (_picked.Contains(index)) _picked.Remove(index);
        else
        {
            if (_picked.Count >= _pickCount) return;
            _picked.Add(index);
        }

        btnConfirm.interactable = (_picked.Count == _pickCount);
    }

    void Confirm()
    {
        var chosen = new List<AccessorySO>();
        foreach (var i in _picked) chosen.Add(_options[i]);

        AccessoryManager.Instance?.EquipMany(chosen);

        panel.SetActive(false);
        _onDone?.Invoke();
    }
}
