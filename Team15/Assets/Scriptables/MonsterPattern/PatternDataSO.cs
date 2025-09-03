using System.Collections;
using UnityEngine;

public abstract class PatternDataSO : ScriptableObject
{
    [Header("PatternInfo")]
    public float damage;

    public abstract IEnumerator Execute(Monster monster);
}
