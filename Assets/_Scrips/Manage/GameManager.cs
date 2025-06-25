using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerData LoadedData;
    public GameObject victoryPopup;
    public GameObject defeatPopup;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("✅ Scene loaded: " + scene.name);
        StartCoroutine(DelaySpawnPlayer());
    }

    public void SpawnAfterSceneLoaded()
    {
        StartCoroutine(DelaySpawnPlayer());
    }

    private IEnumerator DelaySpawnPlayer()
    {
        GameObject playerObj = null;
        GameObject spawnObj = null;

        // Chờ cho tới khi Player và SpawnPoint đều đã tồn tại trong scene
        while (playerObj == null || spawnObj == null) {
            if (playerObj == null && PlayerReferences.Instance != null)
                playerObj = PlayerReferences.Instance.Player;

            if (spawnObj == null)
                spawnObj = GameObject.FindWithTag("PlayerSpawnPoint");

            yield return null;
        }

        playerObj.transform.position = spawnObj.transform.position;
        playerObj.transform.rotation = spawnObj.transform.rotation;

        Debug.Log("✅ Player đã được spawn chính xác.");
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


    public void OnPlayerWin()
    {
        victoryPopup.SetActive(true);
        victoryPopup.GetComponent<PopupAnimator>().ShowFromTop();
        Time.timeScale = 0f; // Dừng thời gian khi thắng
    }

    public void OnPlayerLose()
    {
        defeatPopup.SetActive(true);
        defeatPopup.GetComponent<PopupAnimator>().ShowFromBottom();
        Time.timeScale = 0f; // Dừng thời gian khi thua
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
        DestroyIfExists("InventoryMG");
        // thêm bất kỳ object nào khác bạn cần dọn
    }
}
