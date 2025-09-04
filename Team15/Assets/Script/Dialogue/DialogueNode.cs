using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string label;
    public DialogueNode next;

    [Header("���� ����")]
    public StarterWeaponKind pickWeapon = StarterWeaponKind.None;
    public DialogueNode nextIfSame;
    public bool commitSelectedWeapon = false;
}

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("����")]
    [TextArea(3, 10)] public string[] lines;

    [Header("������")]
    public DialogueChoice[] choices;
}
