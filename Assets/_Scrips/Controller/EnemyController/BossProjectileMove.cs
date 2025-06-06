using System.Collections;
using UnityEngine;

public class BossProjectileMove : MonoBehaviour, IPoolableObject
{
    public float speed = 10f;
    public float lifetime = 2f;

    private bool collided;

    void Start()
    {
        transform.GetComponent<PooledObject>().AutoReturnTime = lifetime;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision co)
    {
        if (!collided) {
            collided = true;
            ContactPoint contact = co.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;

            StartCoroutine(DestroyParticle(0f));
        }
    }
    IEnumerator DestroyParticle(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ObjectPoolManager.Instance.ReturnToPool(gameObject);
    }


    public void OnGet()
    {
    }

    public void OnReturn()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
