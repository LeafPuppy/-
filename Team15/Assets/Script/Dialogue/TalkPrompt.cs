using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkPrompt : MonoBehaviour
{
    [Header("UI 오브젝트")]
    public GameObject promptUI;

    [Header("설정")]
    public string playerTag = "Player";

    bool inRange = false;

    void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        if (promptUI) promptUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = true;
            UpdateUI();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = false;
            UpdateUI();
        }
    }

    void Update()
    {
        if (promptUI)
        {
            bool shouldShow = inRange && !(DialogueUI.Instance && DialogueUI.Instance.IsOpen);
            if (promptUI.activeSelf != shouldShow) promptUI.SetActive(shouldShow);
        }
    }

    void UpdateUI()
    {
        if (!promptUI) return;
        bool show = inRange && !(DialogueUI.Instance && DialogueUI.Instance.IsOpen);
        promptUI.SetActive(show);
    }
}
