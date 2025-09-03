using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [TextArea] [SerializeField] string[] lines;

    public void Interact(Player player)
    {
        if (!DialogueUI.Instance) return;

        if (!DialogueUI.Instance.IsOpen)
            DialogueUI.Instance.Show(lines);
        else
            DialogueUI.Instance.AdvanceOrClose();
    }
}
