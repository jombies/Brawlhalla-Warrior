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
    RoomController Gates;
    int Spawned;
    int AliveMonster;
    bool onChest = false;

    private void Start()
    {
        Gates = transform.GetChild(0).GetComponent<RoomController>();
    }
    private void Update()
    {
        // Kiểm tra điều kiện để mở cổng và sinh rương
        if (Spawned == TotalEnemy && AliveMonster == 0) {
            Gates.GateOpen();
            Gates.DisableGates();
            Destroy(SpawnPos);
            if (!onChest) {
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
    }
    public void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) {
            Debug.LogError("Chưa gán prefab quái vào danh sách!");
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], SpawnPos);
        newEnemy.transform.localPosition = new Vector3(Random.Range(-13, 13), 1, Random.Range(-12, 12));

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
        GameObject newChest = Instantiate(Chest, transform);
        newChest.transform.localPosition = new Vector3(0, yPosOfChest, 0);
    }
    void HandleMonsterDeath()
    {
        AliveMonster--;
    }
}
