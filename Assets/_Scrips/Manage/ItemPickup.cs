using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;
    public void PickUp()
    {
        Debug.Log("Ban vua nhan: " + Item.name);
        bool pickUp = Inventory.Instance.Add(Item);
        if (pickUp) Destroy(gameObject);
    }
}
