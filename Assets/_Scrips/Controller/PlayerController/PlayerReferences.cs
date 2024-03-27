
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    #region location player
    public static PlayerReferences Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public GameObject Player;

    private void Reset()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
}
