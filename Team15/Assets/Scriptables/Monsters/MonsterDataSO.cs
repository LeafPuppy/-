using UnityEngine;

public enum MonsterType
{
    Melee,
    Range,
    Special,
    Boss,
}

[CreateAssetMenu(fileName = "New Monster", menuName = "Entity/Monster")]
public class MonsterDataSO : ScriptableObject
{
    [Header("MonsterInfo")]
    public float maxHealth;
    public float speed;
    public MonsterType type;
}
