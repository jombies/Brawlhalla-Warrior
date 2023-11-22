using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : CharacterState
{
    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.Instance.onEquipchanged += onEquipmentChanged;
    }

    void onEquipmentChanged(Equipment newitem, Equipment oldItem)
    {
        if (newitem != null)
        {
            armor.AddModifier(newitem.Defend);
            damage.AddModifier(newitem.Damage);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.Defend);
            damage.RemoveModifier(oldItem.Damage);
        }
        armor.totalValue();
        damage.totalValue();
    }
}
