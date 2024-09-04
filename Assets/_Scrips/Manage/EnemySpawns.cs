using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawns : MonoBehaviour
{
    [SerializeField] int TotalEnemy;
    [SerializeField] int spawnCount;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] GameObject Chest;
    [SerializeField] Transform SpawnPos;
    [SerializeField] float yPosOfChest;
    GateBehaviour Gates;
    int Spawned;
    int AliveMonster;
    bool onChest = false;

    private void Start()
    {
        Gates = transform.GetChild(0).GetComponent<GateBehaviour>();
    }
    private void Update()
    {
        if (Spawned == TotalEnemy && AliveMonster == 0)
        {
            Gates.GateOpen();
            Gates.DisableGates();
            Destroy(SpawnPos);
            if (!onChest)
            {
                SpawnChest();
            }
        }
    }
    //Thuc hien 
    public void ExecuteSpawn()
    {
        //  InvokeRepeating("SpawnEnemy", 0, 2);
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (Spawned < TotalEnemy)
        {
            if (spawnCount > (TotalEnemy - Spawned))
            {
                spawnCount = (TotalEnemy - Spawned);
            }
            for (int i = 0; i < spawnCount; i++)
            {
                if (Spawned >= TotalEnemy) yield break;
                SpawnEnemy();
                Spawned++;
                AliveMonster++;
            }
            yield return new WaitUntil(() => AliveMonster == 0);
            yield return new WaitForSeconds(1);
        }
    }
    //Sinh ra 
    public void SpawnEnemy()
    {
        // if (--TotalEnemy == 0 || RootPos.childCount == MaxEnemy) CancelInvoke("SpawnEnemy");
        GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], SpawnPos);
        newEnemy.transform.localPosition = new Vector3(Random.Range(-13, 13), 1, Random.Range(-12, 12));
        EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
        stats.OnDeath += HandleMonsterDeath;

    }
    public void SpawnChest()
    {
        onChest = true;
        GameObject newChest = Instantiate(Chest, transform);
        newChest.transform.localPosition = new Vector3(0, yPosOfChest, 0);
    }
    void HandleMonsterDeath()
    {
        AliveMonster--;
    }
}
