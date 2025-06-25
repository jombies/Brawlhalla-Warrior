using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerReferences : MonoBehaviour
{
    #region location player
    public static PlayerReferences Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    public GameObject Player;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Reset()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
