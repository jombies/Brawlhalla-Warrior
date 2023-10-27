
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;
    public Vector3 offet = new Vector3(8, 14, -8);
    private void Update()
    {
        transform.position = target.transform.position + offet;
    }
}
