using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    [SerializeField] ChangeWeapon ChangeWeapon;
    [SerializeField] ChangeHelmet ChangeHelmet;
    [SerializeField] ChangeArmor ChangeArmor;

    Equipment[] currentEquipment;
    Inventory inventory;

    public delegate void OnEquipchanged(Equipment newItem, Equipment oldItem);
    public OnEquipchanged onEquipchanged;
    private void Start()
    {
        inventory = Inventory.Instance;
        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlot];
    }
    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }
        if (onEquipchanged != null)
        {
            onEquipchanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;
        switch (slotIndex)
        {
            case 0:
                ChangeHelmet.changeHelmet(newItem.name);
                break;
            case 1:
                ChangeArmor.changeArmor(newItem.name);
                break;
            case 2:
                ChangeWeapon.changeWeapon(newItem.name);
                break;
            default: Debug.Log("Nothing use"); break;
        }
    }

}