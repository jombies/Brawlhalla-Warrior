using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkin : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] Mesh mesh;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            changemesh();
        }
    }
    void changemesh()
    {
        meshFilter.sharedMesh = mesh;
    }
}
