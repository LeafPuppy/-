using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueNPC : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [TextArea] [SerializeField] string[] lines;
    bool inRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) inRange = true; 
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) inRange = false;
    }

    void Update()
    {
        if (!inRange || Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!DialogueUI.Instance.IsOpen)
            {
                DialogueUI.Instance.Show(lines);
            }
            else
            {
                DialogueUI.Instance.AdvanceOrClose();
            }
        }
    }
}
