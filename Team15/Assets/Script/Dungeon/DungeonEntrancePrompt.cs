using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrancePrompt : MonoBehaviour
{
    [Header("참조UI")]
    public GameObject dungeonEnterUI;

    [Header("플레이어 태그")]
    public string playerTag = "Player";

    void Start()
    {
        if (dungeonEnterUI) dungeonEnterUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            if (dungeonEnterUI) dungeonEnterUI.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            if (dungeonEnterUI) dungeonEnterUI.SetActive(false);
    }
}
