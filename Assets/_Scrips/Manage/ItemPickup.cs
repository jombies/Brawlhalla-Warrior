using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    public void PickUp()
    {
        Debug.Log("Ban vua nhan: " + Item.name);
        bool pickUp = Inventory.Instance.Add(Item);
        if (pickUp) {
            if (TryGetComponent<PooledObject>(out PooledObject pooledObject)) {
                pooledObject.ReturnToPool();
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
