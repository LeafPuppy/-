using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "MobSpawn", menuName = "Pattern/MobSpawn")]
public class MobSpawnPatternSO : PatternDataSO
{
    public int spawnCount;
    public GameObject[] spawnPool;
    private GameObject[] monsters = new GameObject[2];

    public override IEnumerator Execute(Monster monster)
    {
        for(int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = Instantiate(spawnPool[Random.Range(0, spawnPool.Length)], monster.transform.position + new Vector3(i, 0, 0), Quaternion.identity);
        }
        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
        monster.speed = 0;
        monster.canSpawn = false;
        yield return new WaitForSeconds(1.5f);
        monster.speed = monster.data.speed;
        monster.StartCoroutine(monster.CheckInPattern());
    }
}
