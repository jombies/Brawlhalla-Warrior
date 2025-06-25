using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawns : MonoBehaviour
{
    RoomController Gates;
    [Header("Enemy Spawn Settings")]
    [SerializeField] int TotalEnemy;
    [SerializeField] int spawnCount;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] float XPos, ZPos;
    int Spawned;
    int AliveMonster;
    bool onChest = false;

    [Header("Chest Settings")]
    [SerializeField] GameObject Chest;
    [SerializeField] Transform SpawnPos;




    private void Start()
    {
        Gates = GetComponentInChildren<RoomController>();
    }
    //private void Update()
    //{
    //    // Kiểm tra điều kiện để mở cổng và sinh rương
    //    if (Spawned == TotalEnemy && AliveMonster == 0) {
    //        Gates.GateOpen();
    //        Gates.DisableGates();
    //        Destroy(SpawnPos);
    //        if (!onChest) {
    //            SpawnChest();
    //        }
    //    }
    //}
    //Thuc hien 
    public void ExecuteSpawn()
    {
        //  InvokeRepeating("SpawnEnemy", 0, 2);
        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (Spawned < TotalEnemy) {
            // Tính toán số lượng quái cho đợt hiện tại
            int currentSpawnCount = Mathf.Min(spawnCount, TotalEnemy - Spawned);

            for (int i = 0; i < currentSpawnCount; i++) {
                if (Spawned >= TotalEnemy) yield break;
                SpawnEnemy();
                Spawned++;
                AliveMonster++;
            }

            // Đợi cho tới khi tất cả quái trong đợt bị tiêu diệt
            yield return new WaitUntil(() => AliveMonster == 0);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(0.5f);
        Gates.GateOpen();
        if (SpawnPos != null) {
            SpawnPos.gameObject.SetActive(false);
        }

        if (!onChest) {
            SpawnChest();
        }
    }
    public void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) {
            Debug.LogError("Chưa gán prefab quái vào danh sách!");
            return;
        }//GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], SpawnPos);
        Vector3 randomPos = SpawnPos.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        GameObject newEnemy = ObjectPoolManager.Instance.Spawn(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], randomPos);

        // Gán sự kiện xử lý khi quái chết
        EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
        if (stats != null) {
            stats.OnDeath += HandleMonsterDeath;
        }
        else {
            Debug.LogError("Quái thiếu component EnemyStats!");
        }
    }
    public void SpawnChest()
    {
        onChest = true;
        Vector3 pos = SpawnPos.position;
        GameObject newChest = ObjectPoolManager.Instance.Spawn(Chest, pos);
        // newChest.transform.localPosition = new Vector3(0, yPosOfChest, 0);
    }
    void HandleMonsterDeath()
    {
        AliveMonster--;
        GameSummaryManager.Instance.AddEnemyKill();
    }
}
