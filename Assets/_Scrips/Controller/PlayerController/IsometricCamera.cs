using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;

    [Header("Camera Settings")]
    public float angle = 55f;          // Góc nhìn camera
    public float height = 15f;            // Chiều cao so với nhân vật
    public float distance = 10f;          // Độ xa lệch về sau
    public float smoothTime = 0.2f;       // Thời gian mượt khi theo sau

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (target == null && PlayerReferences.Instance != null) {
            target = PlayerReferences.Instance.Player.transform;
        }
        //AudioManager.Instance.PlayMusic("music indie");

    }
    private void OnEnable()
    {
        target = PlayerReferences.Instance.Player.transform;
    }

    public float y, z;
    private void LateUpdate()
    {
        if (target == null) return;
        // Đặt góc nhìn nghiêng kiểu isometric
        transform.rotation = Quaternion.Euler(angle, y, z);

        // Vị trí camera lệch sau và trên nhân vật
        Vector3 offset = new Vector3(0f, height, -distance);
        Vector3 desiredPosition = target.position + offset;

        // Camera mượt mà di chuyển theo nhân vật
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
