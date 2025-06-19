using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item _item;
    Button button;
    public Image IconPlus;
    public Image Icon;
    public Button RemoveBtn;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UseItem);
    }
    public void AddItem(Item newItem)
    {
        _item = newItem;
        Icon.sprite = _item.Icon;
        Icon.enabled = true;
        RemoveBtn.interactable = true;
        IconPlus.enabled = false;
    }
    public void RemoveItem()
    {
        _item = null;
        Icon.sprite = null;
        Icon.enabled = false;
        RemoveBtn.interactable = false;
        IconPlus.enabled = true;
    }

    public void UseItem()
    {
        if (_item != null) {
            _item.Use();
            this.PostEvent(EventID.OnUseItem);
        }
    }
    public void OnRemove()
    {
        Inventory.Instance.Remove(_item);
    }
    private void Reset()
    {
        button = GetComponent<Button>();
        IconPlus = transform.GetChild(0).GetComponent<Image>();
        Icon = transform.GetChild(1).GetComponent<Image>();
        RemoveBtn = transform.GetChild(2).GetComponent<Button>();
    }
}
