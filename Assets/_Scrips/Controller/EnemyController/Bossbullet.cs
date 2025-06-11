using System.Collections;
using UnityEngine;

public class Bossbullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage;
    private bool collided;

    void Start()
    {
        GetComponent<PooledObject>().SetAutoReturnTime(lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("Bullet hit the player!");
            other.GetComponent<CharacterStat>().TakeDamage(damage);
            GetComponent<PooledObject>().ReturnToPool();
        }
    }
    //private void OnCollisionEnter(Collision co)
    //{
    //    if (!collided) {
    //        collided = true;
    //        ContactPoint contact = co.contacts[0];
    //        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //        Vector3 pos = contact.point;

    //        StartCoroutine(DestroyParticle(0f));
    //    }
    //}
}
