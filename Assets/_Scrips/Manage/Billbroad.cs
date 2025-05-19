using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject Camera;
    private void Start()
    {
        Camera = UnityEngine.Camera.main.gameObject;
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.transform.forward);
    }
}
