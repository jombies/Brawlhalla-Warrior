using UnityEngine;
using UnityEngine.Rendering;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager Instance;
    private void Awake()
    {
        Instance = this;
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
    //void LoadEquip()
    //{
    //    _changeWeapon = FindAnyObjectByType<ChangeWeapon>();
    //    _changeArmor = FindAnyObjectByType<ChangeArmor>();
    //    _changeHelmet = FindAnyObjectByType<ChangeHelmet>();
    //}
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.EquipSlot;
        Equipment oldItem = null;

        if (_currentEquipment[slotIndex] != null)
        {
            oldItem = _currentEquipment[slotIndex];
            _inventory.Add(oldItem);
        }
        if (onEquipchanged != null)
        {
            onEquipchanged.Invoke(newItem, oldItem);
        }

        _currentEquipment[slotIndex] = newItem;
        switch (slotIndex)
        {
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
}