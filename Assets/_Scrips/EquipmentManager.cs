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
        currentEquipment[slotIndex] = newItem;
        //change helmet
        if (slotIndex == 0) ChangeHelmet.changeHelmet(newItem.name);
        //armor
        if (slotIndex == 1) ChangeArmor.changeArmor(newItem.name);
        //change weapon
        ChangeWeapon.changeWeapon(newItem.name);
    }

}