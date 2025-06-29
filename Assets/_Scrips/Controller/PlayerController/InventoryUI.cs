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
    [SerializeField] TextMeshProUGUI _textCoin;

    GamePlayUI _gamePlayUI;

    // Start is called before the first frame update
    void Start()
    {
        Inventory.Instance.OnItemChangedCallBack += UpdateUi;
        _gamePlayUI = GetComponent<GamePlayUI>();
        _slots = ItemParrent.GetComponentsInChildren<InventorySlot>();
        _invetoryui.SetActive(false);
        _ivenBtn.onClick.AddListener(() => _invetoryui.SetActive(!_invetoryui.activeSelf));
    }
    private void OnEnable()
    {
        this.RegisterListener(EventID.OnUseItem, (e) => UpdateInfo(PlayerDataManager.Load()));
        this.RegisterListener(EventID.OnCoinCollected, UpdateCointext);
        Inventory.Instance.OnItemChangedCallBack += UpdateUi;
    }

    private void OnDisable()
    {
        if (Inventory.Instance != null)
            Inventory.Instance.OnItemChangedCallBack -= UpdateUi;
        this.RemoveListener(EventID.OnCoinCollected, UpdateCointext);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_gamePlayUI.isPause) {
            if (GameManager.Instance.victoryPopup.activeSelf || GameManager.Instance.defeatPopup.activeSelf) return;
            _invetoryui.SetActive(!_invetoryui.activeSelf);
            Time.timeScale = _invetoryui.activeSelf ? 0 : 1;
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
    public void UpdateCointext(object obj)
    {
        _textCoin.text = obj.ToString();
    }
    public void UpdateInfo(PlayerData data)
    {
        levelText.text = $"Level.{data.level}";
        healthText.text = PlayerReferences.Instance.Player.GetComponent<PlayerStat>().currentHealth.ToString();
        armorText.text = PlayerReferences.Instance.Player.GetComponent<PlayerStat>().Armor.Value.ToString();
        attackText.text = PlayerReferences.Instance.Player.GetComponent<PlayerStat>().Damage.Value.ToString();
        CoinText.text = Inventory.Instance.coin.ToString();
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnUseItem, (e) => UpdateInfo(PlayerDataManager.Load()));
    }
    private void Reset()
    {
        _ivenBtn = transform.GetChild(0).GetComponent<Button>();
    }
}