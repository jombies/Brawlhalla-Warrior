using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData LoadedData;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void PrepareDataBeforePlay()
    {
        LoadedData = PlayerDataManager.Load();
    }

    public void SpawnAfterSceneLoaded()
    {
        StartCoroutine(DelaySpawnPlayer());
    }

    private IEnumerator DelaySpawnPlayer()
    {
        yield return null; // đợi 1 frame để scene load xong
        SpawnPlayerAtSceneSpawnPoint();
    }

    public void SpawnPlayerAtSceneSpawnPoint()
    {
        var Player = PlayerReferences.Instance.Player;
        Transform spawnPoint = GameObject.FindWithTag("PlayerSpawnPoint")?.transform;

        if (spawnPoint != null) {
            Player.transform.position = spawnPoint.position;
            Player.transform.rotation = spawnPoint.rotation;
            Debug.Log("Đã dịch chuyển nhân vật đến điểm spawn.");
        }
        else {
            Debug.LogWarning("Không tìm thấy PlayerSpawnPoint trong scene hiện tại.");
        }
    }
    void DestroyIfExists(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null) {
            Destroy(obj);
            Debug.Log("Đã xoá object: " + objectName);
        }
    }

    public void CleanUpSystemsForHome()
    {
        DestroyIfExists("CanvasGameUI");
        DestroyIfExists("InputSingleton");
        DestroyIfExists("===Player===");
        DestroyIfExists("EquipmentManager");
        DestroyIfExists("ObjectPoolManager");
        // thêm bất kỳ object nào khác bạn cần dọn
    }
}
