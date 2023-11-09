using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    public Transform itemParrent;
    inventorySlot[] slots;
    [SerializeField] Button ivenBtn;
    [SerializeField] GameObject invetoryui;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallBack += UpdateUI;
        slots = itemParrent.GetComponentsInChildren<inventorySlot>();
        invetoryui.SetActive(false);
        ivenBtn.onClick.AddListener(() => invetoryui.SetActive(!invetoryui.activeSelf));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            invetoryui.SetActive(!invetoryui.activeSelf);
        }
    }
    void UpdateUI()
    {
        Debug.Log("Update UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else slots[i].RemoveItem();
        }

    }
}
