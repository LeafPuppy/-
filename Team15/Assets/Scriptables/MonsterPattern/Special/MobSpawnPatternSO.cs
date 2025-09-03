using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MobSpawn", menuName = "Pattern/MobSpawn")]
public class MobSpawnPatternSO : PatternDataSO
{
    public int spawnCount;
    public float spawnTime;
    public GameObject[] spawnPool;
    private GameObject[] monsters = new GameObject[2];

    public override IEnumerator Execute(Monster monster)
    {
        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = Instantiate(spawnPool[Random.Range(0, spawnPool.Length)], monster.transform.position + new Vector3(i, 0, 0), Quaternion.identity);
        }
        monster.canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
        monster.StartCoroutine(monster.CheckInPattern());
    }
}
