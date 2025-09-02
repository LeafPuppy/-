using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Entity/Monster")]
public class MonsterDataSO : ScriptableObject
{
    [Header("MonsterInfo")]
    public float maxHealth;
    public float speed;
}
