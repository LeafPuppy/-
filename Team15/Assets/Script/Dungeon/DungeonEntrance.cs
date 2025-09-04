using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour, IInteractable
{
    [Header("참조")]
    public DifficultyUI difficultyUI;
    public DungeonEntrancePrompt prompt;

    void Reset() { if (!prompt) prompt = GetComponent<DungeonEntrancePrompt>(); }

    public void Interact(Player player)
    {
        GetComponent<DungeonEntrancePrompt>()?.dungeonEnterUI?.SetActive(false);

        if (difficultyUI != null)
        {
            difficultyUI.Open();
        }
    }
}
