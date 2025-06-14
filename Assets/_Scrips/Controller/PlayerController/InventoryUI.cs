using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory UI Elements")]
    InventorySlot[] _slots;
    [SerializeField] Button _ivenBtn;
    [SerializeField] GameObject _invetoryui;
    [SerializeField] Transform ItemParrent;

    [Header("Info")]
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI armorText;
    [SerializeField] TextMeshProUGUI attackText;
    [SerializeField] TextMeshProUGUI CoinText;


    // Start is called before the first frame update
    void Start()
    {

        Inventory.Instance.OnItemChangedCallBack += UpdateUi;
        _slots = ItemParrent.GetComponentsInChildren<InventorySlot>();
        _invetoryui.SetActive(false);
        _ivenBtn.onClick.AddListener(() => _invetoryui.SetActive(!_invetoryui.activeSelf));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            _invetoryui.SetActive(!_invetoryui.activeSelf);
            UpdateInfo(PlayerDataManager.Load());
        }
    }
    void UpdateUi()
    {
        Debug.Log("Update UI");
        for (int i = 0; i < _slots.Length; i++) {
            if (i < Inventory.Instance.Items.Count) {
                _slots[i].AddItem(Inventory.Instance.Items[i]);
            }
            else _slots[i].RemoveItem();
        }
    }
    public void UpdateInfo(PlayerData data)
    {
        levelText.text = $"Level.{data.level}";
        healthText.text = PlayerReferences.Instance.Player.GetComponent<PlayerStat>().currentHealth.ToString();
        armorText.text = data.armor.ToString();
        attackText.text = data.attack.ToString();
        CoinText.text = Inventory.Instance.coin.ToString();
    }
    private void Reset()
    {
        _ivenBtn = transform.GetChild(0).GetComponent<Button>();
        _invetoryui = transform.GetChild(1).gameObject;
        ItemParrent = _invetoryui.transform.GetChild(0);
        healthText = _invetoryui.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        armorText = _invetoryui.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        attackText = _invetoryui.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        CoinText = _invetoryui.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
    }
}