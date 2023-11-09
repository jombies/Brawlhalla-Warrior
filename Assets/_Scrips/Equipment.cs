using UnityEngine;

[CreateAssetMenu(fileName = "new Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public int Defend;
    public int Damage;

    public override void Use()
    {
        base.Use();
        EquipmentManager.Instance.Equip(this);
        RemoveItem();
    }
}

public enum EquipmentSlot { Head, Armor, Leg, Weapon, health, key }
