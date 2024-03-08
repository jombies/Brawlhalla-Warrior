public class PlayerStat : CharacterStat
{
    CharacterAnimation Animator;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<CharacterAnimation>();
        EquipmentManager.Instance.onEquipchanged += OnEquipmentChanged;
    }

    void OnEquipmentChanged(Equipment newitem, Equipment oldItem)
    {
        if (newitem != null)
        {
            Armor.AddModifier(newitem.Defend);
            Damage.AddModifier(newitem.Damage);
        }
        if (oldItem != null)
        {
            Armor.RemoveModifier(oldItem.Defend);
            Damage.RemoveModifier(oldItem.Damage);
        }
        Armor.TotalValue();
        Damage.TotalValue();
    }

    public override void Die()
    {
        base.Die();
        Animator.GetDie();
    }
}
