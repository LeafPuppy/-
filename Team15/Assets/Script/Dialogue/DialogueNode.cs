using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string label;
    public DialogueNode next;
}

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("����")]
    [TextArea(3, 10)] public string[] lines;

    [Header("������")]
    public DialogueChoice[] choices;
}
