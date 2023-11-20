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
    public ChangeWeapon ChangeWeapon;
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

        ChangeWeapon.GetnameWeapon(newItem.name);
    }

}