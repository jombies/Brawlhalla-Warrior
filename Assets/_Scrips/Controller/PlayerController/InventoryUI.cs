using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    Inventory _inventory;
    InventorySlot[] _slots;
    [SerializeField] Button _ivenBtn;
    [SerializeField] GameObject _invetoryui;
    [SerializeField] Transform ItemParrent;

    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.Instance;
        _inventory.OnItemChangedCallBack += UpdateUi;
        _slots = ItemParrent.GetComponentsInChildren<InventorySlot>();
        _invetoryui.SetActive(false);
        _ivenBtn.onClick.AddListener(() => _invetoryui.SetActive(!_invetoryui.activeSelf));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _invetoryui.SetActive(!_invetoryui.activeSelf);
        }
    }
    void UpdateUi()
    {
        Debug.Log("Update UI");
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < _inventory.Items.Count)
            {
                _slots[i].AddItem(_inventory.Items[i]);
            }
            else _slots[i].RemoveItem();
        }

    }
}
