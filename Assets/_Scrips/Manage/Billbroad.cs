using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        Camera Camera = CameraController.Instance.GetCamera();
        transform.LookAt(transform.position + Camera.transform.forward);
    }
}
