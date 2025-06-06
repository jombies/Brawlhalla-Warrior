using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeParticle : MonoBehaviour
{
    ParticleSystem firering;
    SphereCollider sphereCollider;
    public float radius;
    void Start()
    {
        firering = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void getSphere()
    {

    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("Player hit by particle");
        }
    }
    private void OnParticleTrigger()
    {
        Debug.Log("Player hit by particle");
    }
}
