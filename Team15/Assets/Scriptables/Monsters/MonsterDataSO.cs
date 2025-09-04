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
    public float chasingRange;
    public MonsterType type;

    //무기 추가되면 그 때 수정
    //[Header("WeaponInfo")]
    //무기정보
}
