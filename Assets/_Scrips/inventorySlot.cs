using UnityEngine;
using UnityEngine.UI;

public class inventorySlot : MonoBehaviour
{
    Item item;
    public Image icon;
    public Button removeBtn;
    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeBtn.interactable = true;
    }
    public void RemoveItem()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeBtn.interactable = false;
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
    public void OnRemove()
    {
        Inventory.Instance.Remove(item);
    }
}
