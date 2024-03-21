using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _target;
    public float Speed = 5.0f;
    public Vector3 Offet = new(8, 14, -8);

    private void Start()
    {
        _target = PlayerReferences.Instance.Player.transform;
    }
    private void Update()
    {
        transform.position = _target.transform.position + Offet;
    }
}
