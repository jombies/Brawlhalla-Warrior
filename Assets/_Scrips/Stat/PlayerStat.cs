using System.Collections;
using UnityEngine;

public class PlayerStat : CharacterStat
{
    CharacterAnimation Animator;
    [SerializeField] StatPlayerUI StatPlayerUI;

    [Header("Shield")]
    int MaxShield = 100;
    public int currentShield { get; private set; }
    private Coroutine shieldRegenCoroutine;
    private float shieldRegenDelay = 10f;


    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<CharacterAnimation>();
        EquipmentManager.Instance.onEquipchanged += OnEquipmentChanged;
        MaxShield = Armor.Value;
        currentShield = MaxShield;
    }

    void OnEquipmentChanged(Equipment newitem, Equipment oldItem)
    {
        if (newitem != null) {
            Armor.AddModifier(newitem.Defend);
            Damage.AddModifier(newitem.Damage);
        }
        if (oldItem != null) {
            Armor.RemoveModifier(oldItem.Defend);
            Damage.RemoveModifier(oldItem.Damage);
        }
        //Armor.TotalValue();
        //Damage.TotalValue();
        Debug.Log("Armor: " + Armor.Value);
        Debug.Log("Damage: " + Damage.Value);
    }

    protected override void OnTakeDamage(int dmg)
    {

        if (shieldRegenCoroutine != null)
            StopCoroutine(shieldRegenCoroutine);
        shieldRegenCoroutine = StartCoroutine(RegenerateShield());

        if (currentShield > 0) {
            int shieldDmg = Mathf.Min(currentShield, dmg);
            currentShield -= shieldDmg;
            dmg -= shieldDmg;
            StatPlayerUI?.setShield(currentShield);
        }
        if (dmg > 0) {
            currentHealth -= dmg;
            StatPlayerUI?.setHealth(currentHealth);
            if (currentHealth <= 0 && !isDead) {
                Die();
                isDead = true;
            }
            AudioManager.Instance.PlaySFX("player hit");
        }

    }

    public void Healing(int value)
    {
        currentHealth += MaxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        StatPlayerUI.setHealth(currentHealth);
    }
    protected override void Die()
    {
        // base.Die();
        Animator.GetDie();
    }
    IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(shieldRegenDelay);

        while (currentShield < MaxShield) {
            yield return new WaitForSeconds(1f);
            currentShield += 1;
            StatPlayerUI?.setShield(currentShield);
        }
        StatPlayerUI?.setShield(currentShield);
        Debug.Log("RegenerateShield");
    }
}
