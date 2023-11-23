
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    #region location player
    public static PlayerLocation Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    public GameObject player;

    private void Start()
    {
        Application.targetFrameRate = 50;
    }
}
