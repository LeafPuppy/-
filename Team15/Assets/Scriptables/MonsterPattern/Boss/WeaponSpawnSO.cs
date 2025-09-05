using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpawn", menuName = "Pattern/WeaponSpawn")]
public class WeaponSpawnSO : PatternDataSO
{
    public int spawnNum;
    public GameObject[] weaponPrefabs;

    public override IEnumerator Execute(Monster monster)
    {
        monster.inPattern = true;
        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
        AudioManager.Instance.PlaySFX("SwaySFX");
        for (int i = 0; i < spawnNum; i++)
        {
            var go = Instantiate(weaponPrefabs[Random.Range(0, weaponPrefabs.Length)], monster.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            var rg = go.GetComponent<Rigidbody2D>();
            rg.AddForce(new Vector2(Random.Range(-3, 3), 5), ForceMode2D.Impulse);
            go.AddComponent<WeaponDropDamage>();
            var wdd = go.GetComponent<WeaponDropDamage>();
            wdd.damage = damage;
        }
        yield return new WaitForSeconds(2f);
        monster.stateMachine.ChangeState(monster.stateMachine.AttackState);
        monster.canSpawn = false;
        monster.StartCoroutine(monster.CheckInPattern());
    }
}
