using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item _item;
    public Image Icon;
    public Button RemoveBtn;
    public void AddItem(Item newItem)
    {
        _item = newItem;
        Icon.sprite = _item.Icon;
        Icon.enabled = true;
        RemoveBtn.interactable = true;
    }
    public void RemoveItem()
    {
        _item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        RemoveBtn.interactable = false;
    }

    public void UseItem()
    {
        if (_item != null)
        {
            _item.Use();
        }
    }
    public void OnRemove()
    {
        Inventory.Instance.Remove(_item);
    }
}
