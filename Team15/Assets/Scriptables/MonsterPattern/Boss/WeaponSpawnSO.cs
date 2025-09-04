using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpawn", menuName = "Pattern/WeaponSpawn")]
public class WeaponSpawnSO : PatternDataSO
{
    public int spawnNum;
    public GameObject[] weaponPrefabs;

    public override IEnumerator Execute(Monster monster)
    {
        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
        for (int i = 0; i < spawnNum; i++)
        {
            var go = Instantiate(weaponPrefabs[Random.Range(0, weaponPrefabs.Length)], monster.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            var rg = go.GetComponent<Rigidbody2D>();
            rg.AddForce(new Vector2(Random.Range(-3, 3), 1) * Random.Range(1, 10), ForceMode2D.Impulse);
            go.AddComponent<WeaponDropDamage>();
            var wdd = go.GetComponent<WeaponDropDamage>();
            wdd.damage = damage;
        }
        yield return new WaitForSeconds(2f);
        monster.canSpawn = false;
        monster.StartCoroutine(monster.CheckInPattern());
    }
}
