using UnityEngine;


public class Interactable : MonoBehaviour
{
    float radiusOrigin = 1.8f;
    SphereCollider spCollider;
    private void Awake()
    {
        spCollider = GetComponent<SphereCollider>();
        radiusOrigin = spCollider.radius;
    }
    void Setradius(float radius)
    {
        spCollider.radius = radius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            Inventory.Instance.coinCollected(other.gameObject);
        }
        if (other.gameObject.tag == "Health" || other.gameObject.tag == "Weapon" || other.gameObject.tag == "Head")
        {
            other.gameObject.GetComponent<ItemPickup>().pickUp();
        }

    }
}
