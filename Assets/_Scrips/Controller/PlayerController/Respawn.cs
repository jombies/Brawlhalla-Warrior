using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    GameObject player;
    public Transform respawnPoint;
    public float respawnValue = -10;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>().gameObject;

    }
    private void OnEnable()
    {
        player = GetComponent<PlayerController>().gameObject;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadElement());
    }
    private void Update()
    {
        StartCoroutine(LoadElement());
    }
    public IEnumerator LoadElement()
    {
        yield return null;
        respawnPoint = GameObject.Find("PlayerSpawnPoint")?.transform;
        if (respawnPoint == null) {
            respawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        }
        else if (player.transform.position.y < respawnValue) {
            player.transform.position = respawnPoint.position;
        }
    }
}