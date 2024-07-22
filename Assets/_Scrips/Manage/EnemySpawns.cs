using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawns : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] int counter;
    [SerializeField] Transform RootPos;

    // Start is called before the first frame update
    void Start()
    {
    }
    public void ExecuteSpawn()
    {
        InvokeRepeating("SpawnEnemy", 0, 2);
    }
    public void SpawnEnemy()
    {
        if (--counter == 0) CancelInvoke("SpawnEnemy");
        GameObject newGameObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], RootPos);
        newGameObject.transform.localPosition = new Vector3(Random.Range(-14, 14), 1, Random.Range(-12, 12));
    }
}
