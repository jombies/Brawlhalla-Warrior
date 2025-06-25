using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    [SerializeField] ChangeWeapon _changeWeapon;
    [SerializeField] ChangeHelmet _changeHelmet;
    [SerializeField] ChangeArmor _changeArmor;
    Equipment[] _currentEquipment;
    Inventory _inventory;

    public delegate void OnEquipchanged(Equipment newItem, Equipment oldItem);
    public OnEquipchanged onEquipchanged;

    private void Start()
    {
        _inventory = Inventory.Instance;
        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        _currentEquipment = new Equipment[numSlot];
        //LoadEquip();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadEquip());
    }
    IEnumerator LoadEquip()
    {
        yield return new WaitForSeconds(0.5f); // Wait for the scene to load completely
        _changeWeapon = GameObject.FindWithTag(tag: "Change weapon").GetComponent<ChangeWeapon>();
        _changeArmor = GameObject.FindWithTag(tag: "Change armor").GetComponent<ChangeArmor>();
        _changeHelmet = GameObject.FindWithTag(tag: "Change helmet").GetComponent<ChangeHelmet>();
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.EquipSlot;
        Equipment oldItem = null;

        if (_currentEquipment[slotIndex] != null) {
            oldItem = _currentEquipment[slotIndex];
            _inventory.Add(oldItem);
        }
        if (onEquipchanged != null) {
            onEquipchanged.Invoke(newItem, oldItem);
        }

        _currentEquipment[slotIndex] = newItem;
        switch (slotIndex) {
            case 0:
                _changeHelmet.changeHelmet(newItem.name);
                break;
            case 1:
                _changeArmor.changeArmor(newItem.name);
                break;
            case 2:
                _changeWeapon.changeWeapon(newItem.name);
                break;
            default: Debug.Log("Nothing use"); break;
        }

    }
    public void Suplly(SupllyEquipment s)
    {
        PlayerReferences.Instance.Player.GetComponent<PlayerStat>().Healing(s.HP);
    }

    private void Reset()
    {
        LoadEquip();
    }
}