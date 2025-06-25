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
        if (other.gameObject.tag == "coin") {
            AudioManager.Instance.PlaySFX("coin");
            Inventory.Instance.CoinCollected(other.gameObject);
        }
        if (other.gameObject.tag == "Weapon" || other.gameObject.tag == "Head" || other.gameObject.tag == "Helmet" || other.gameObject.tag == "Armor") {
            AudioManager.Instance.PlaySFX("item pickup");
            other.gameObject.GetComponent<ItemPickup>().PickUp();
        }
        if (other.gameObject.tag == "HP") {
            AudioManager.Instance.PlaySFX("buff");
            ObjectPoolManager.Instance.Despawn(other.gameObject);
            PlayerReferences.Instance.Player.GetComponent<PlayerStat>().Healing(999);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Health") {
            AudioManager.Instance.PlaySFX("buff");
            other.gameObject.GetComponent<ItemPickup>().PickUp();
        }

    }
}
