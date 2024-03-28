using UnityEngine;

public class NEWCAMERA : MonoBehaviour
{
    public Transform player;
    Vector3 _currVelocity = Vector3.zero;
    public Vector3 Orbit = new(6.1f, 10, -6.1f);
    public float smoothTime = .2f;

    private void LateUpdate()
    {
        Vector3 Target = player.position + Orbit;
        transform.rotation = Quaternion.Euler(50, -45, 0);
        transform.position = Vector3.SmoothDamp(transform.position, Target, ref _currVelocity, smoothTime);
    }

    private void Start()
    {
        player = PlayerReferences.Instance.Player.transform;
    }
}
