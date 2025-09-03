using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text bodyText;

    string[] lines;
    int index = -1;

    void Awake()
    {
        Instance = this;
        if (panel) panel.SetActive(false);
    }

    public bool IsOpen => panel && panel.activeSelf;

    public void Show(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;

        lines = newLines;
        index = 0;

        panel.SetActive(true);
        if (bodyText) bodyText.text = lines[index];
    }

    public void AdvanceOrClose()
    {
        if (!IsOpen || lines == null) return;

        if (index >= lines.Length - 1) { Hide(); return; }

        index++;
        if (bodyText) bodyText.text = lines[index];
    }

    public void Hide()
    {
        panel?.SetActive(false);
        lines = null;
        index = -1;
    }

    private void Update()
    {
        if (!IsOpen || Keyboard.current == null) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame) Hide();
    }
}
