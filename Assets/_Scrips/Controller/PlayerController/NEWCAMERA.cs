using UnityEngine;

public class NEWCAMERA : MonoBehaviour
{
    public Transform player;
    Vector3 Orbit = new(0, 5, 0);
    private void LateUpdate()
    {
        transform.position = player.position + Orbit;
    }

    private void Start()
    {
        player = PlayerReferences.Instance.Player.transform;
    }
}
