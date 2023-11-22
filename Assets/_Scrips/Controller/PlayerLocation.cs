
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public static PlayerLocation Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject player;
}
