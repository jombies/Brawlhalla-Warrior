using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public void pickUp()
    {
        Debug.Log("Ban vua nhan: " + item.name);
        bool pickUp = Inventory.Instance.Add(item);
        if (pickUp) Destroy(gameObject);
    }
}
