using UnityEngine;

[CreateAssetMenu(fileName = "new Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{

    public EquipmentSlot EquipSlot;
    public int Defend;
    public int Damage;

    public override void Use()
    {
        EquipmentManager.Instance.Equip(this);

        RemoveItem();
    }
}
public enum EquipmentSlot { Head, Armor, Weapon }

