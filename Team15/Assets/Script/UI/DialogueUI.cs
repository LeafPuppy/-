using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("참조")]
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text bodyText;

    [Header("선택UI")]
    [SerializeField] RectTransform choicesRoot;
    [SerializeField] Button choiceButtonPrefab;

    DialogueNode current;
    int index = -1;
    bool choosing = false;
    readonly List<Button> activeButtons = new();

    float prevTimeScale = 1f;
    bool pausedByDialogue = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (panel) panel.SetActive(false);
        if (choicesRoot) choicesRoot.gameObject.SetActive(false);
    }

    public bool IsOpen => panel && panel.activeSelf;

    public void Show(DialogueNode start)
    {
        if (!start) return;

        current = start;
        index = 0;
        choosing = false;
        ClearChoices();

        panel.SetActive(true);
        bodyText.text = (current.lines != null && current.lines.Length > 0) ? current.lines[0] : "";

        if (!pausedByDialogue)
        {
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            pausedByDialogue = true;
        }
    }

    public void OnInteractPressed()
    {
        if (!IsOpen) return;
        if (choosing) return;
        AdvanceOrClose();
    }

    public void AdvanceOrClose()
    {
        if (!current) { Hide(); return; }

        index++;
        if (current.lines != null && index < current.lines.Length)
        {
            bodyText.text = current.lines[index];
            return;
        }

        var choices = current.choices;
        if (choices != null && choices.Length > 0)
        {
            ShowChoices(choices);
        }
        else Hide();
    }

    void ShowChoices(DialogueChoice[] choices)
    {
        choosing = true;
        ClearChoices();
        choicesRoot.gameObject.SetActive(true);

        int count = Mathf.Min(3, choices.Length);
        for (int i = 0; i < count; i++)
        {
            SpawnChoice(choices[i]);
        }
    }

    public void SpawnChoice(DialogueChoice c)
    {
        var btn = Instantiate(choiceButtonPrefab, choicesRoot);
        var text = btn.GetComponentInChildren<TMP_Text>(true);
        if (text) text.text = c.label;

        btn.onClick.AddListener(() =>
        {
            choosing = false;
            ClearChoices();
            if (c.next) Show(c.next);
            else Hide();
        });

        activeButtons.Add(btn);
    }

    void ClearChoices()
    {
        foreach (var b in activeButtons)
        {
            if (!b) continue;
            b.onClick.RemoveAllListeners();
            Destroy(b.gameObject);
        }
        activeButtons.Clear();
        if (choicesRoot) choicesRoot.gameObject.SetActive(false);
    }

    public void Hide()
    {
        panel?.SetActive(false);
        current = null;
        index = -1;
        choosing = false;
        ClearChoices();

        if (pausedByDialogue)
        {
            Time.timeScale = (prevTimeScale <= 0f) ? 1f : prevTimeScale;
            pausedByDialogue = false;
        }
    }

    private void Update()
    {
        if (!IsOpen || Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Hide();
    }
}
