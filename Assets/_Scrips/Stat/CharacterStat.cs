using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [Header("Stat")]
    public int MaxHealth;
    public Stat Damage;
    public Stat Armor;

    //[Header("RumTime")]
    public int currentHealth { get; protected set; }
    public bool isDead = false;

    //bool check;
    protected virtual void Awake()
    {
        currentHealth = MaxHealth;
    }

    public virtual void TakeDamage(int dmg)
    {
        if (isDead) return;
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);
        Debug.Log(gameObject.name + "dame: " + dmg);
        OnTakeDamage(dmg);
    }
    protected virtual void OnTakeDamage(int Finaldmg) { }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} Die!");
    }
}
