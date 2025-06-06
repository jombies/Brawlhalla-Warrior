using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour, IPoolableObject
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PooledObject>().AutoReturnTime = lifetime;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (speed != 0) {
            //rb.velocity = transform.forward * speed;
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            Debug.Log("Bullet hit the player!");
            other.GetComponent<CharacterStat>().TakeDamage(damage); // Example damage value
            GetComponent<PooledObject>().ReturnToPool(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

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
