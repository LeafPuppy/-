using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MobSpawn", menuName = "Pattern/MobSpawn")]
public class MobSpawnPatternSO : PatternDataSO
{
    public int spawnCount;
    public float spawnTime;
    public GameObject[] spawnPool;
    public Vector2[] spawnPositions;
    private GameObject[] monsters = new GameObject[2];
    private Vector2[] positions = new Vector2[2];
    

    public override IEnumerator Execute(Monster monster)
    {
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = spawnPositions[Random.Range(0, spawnPositions.Length)];
            while(i != 0 &&spawnPositions[i] == spawnPositions[i - 1])
            {
                positions[i] = spawnPositions[Random.Range(0, spawnPositions.Length)];
            }
        }
        monster.stateMachine.ChangeState(monster.stateMachine.SturnState);
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i] = Instantiate(spawnPool[Random.Range(0, spawnPool.Length)], positions[i], Quaternion.identity);
        }
        monster.canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        monster.stateMachine.ChangeState(monster.stateMachine.IdleState);
        monster.StartCoroutine(monster.CheckInPattern());
    }
}
