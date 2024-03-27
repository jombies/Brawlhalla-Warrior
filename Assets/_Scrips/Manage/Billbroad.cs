using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject Camera;
    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.transform.forward);
    }
}
