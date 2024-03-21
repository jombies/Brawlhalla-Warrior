using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "coin")
        {
            Inventory.Instance.CoinCollected(other.gameObject);
        }
        if (other.gameObject.tag == "Health" || other.gameObject.tag == "Weapon" || other.gameObject.tag == "Head" || other.gameObject.tag == "Helmet" || other.gameObject.tag == "Armor")
        {
            other.gameObject.GetComponent<ItemPickup>().PickUp();
        }
        if (other.gameObject.tag == "HP")
        {
            PlayerReferences.Instance.Player.GetComponent<PlayerStat>().Healing(100);
            Destroy(other.gameObject);
        }
    }
}
