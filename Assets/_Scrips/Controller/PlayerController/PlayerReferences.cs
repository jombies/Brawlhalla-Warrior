
using UnityEngine;

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

    private void Reset()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
