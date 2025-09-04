using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string label;
    public DialogueNode next;

    [Header("무기 선택")]
    public StarterWeaponKind pickWeapon = StarterWeaponKind.None;
    public DialogueNode nextIfSame;
    public bool commitSelectedWeapon = false;
}

[CreateAssetMenu(menuName = "Dialogue/Node")]
public class DialogueNode : ScriptableObject
{
    [Header("본문")]
    [TextArea(3, 10)] public string[] lines;

    [Header("선택지")]
    public DialogueChoice[] choices;
}
