using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpawn", menuName = "Pattern/WeaponSpawn")]
public class WeaponSpawnSO : PatternDataSO
{
    public GameObject[] weaponPrefabs;

    public override IEnumerator Execute(Monster monster)
    {
        throw new System.NotImplementedException();
    }
}
