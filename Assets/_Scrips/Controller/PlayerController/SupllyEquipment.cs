using UnityEngine;

[CreateAssetMenu(fileName = "new Equipment", menuName = "Inventory/Suplly")]
public class SupllyEquipment : Item
{
    public int HP;
    public override void Use()
    {
        AudioManager.Instance.PlaySFX("health pot");
        EquipmentManager.Instance.Suplly(this);
        RemoveItem();
    }
}


