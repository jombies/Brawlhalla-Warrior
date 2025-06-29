using UnityEngine;

[CreateAssetMenu(fileName = "new Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{

    public EquipmentType EquipType;
    public int Defend;
    public int Damage;

    public override void Use()
    {
        AudioManager.Instance.PlaySFX("item pickup");
        EquipmentManager.Instance.Equip(this);
        RemoveItem();
    }
}
public enum EquipmentType { Head, Armor, Weapon }

