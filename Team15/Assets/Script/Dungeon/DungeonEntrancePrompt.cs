using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrancePrompt : MonoBehaviour
{
    [Header("참조UI")]
    public GameObject enterPanel;

    [Header("플레이어 태그")]
    public string playerTag = "Player";

    void Start()
    {
        if (enterPanel) enterPanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            if (enterPanel) enterPanel.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            if (enterPanel) enterPanel.SetActive(false);
    }

    public void Hide()
    {
        if (enterPanel) enterPanel.SetActive(false);
    }
}
