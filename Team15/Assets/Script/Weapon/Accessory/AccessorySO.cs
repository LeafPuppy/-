using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccessoryType { Boots, Stone, Coffee, Magnifier, RubberBand, SecondHeart }

[CreateAssetMenu(menuName = "Game/Accessory", fileName = "ACC_")]
public class AccessorySO : ScriptableObject
{
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
    public AccessoryType type;

    public bool stackable = true;
}
