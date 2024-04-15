using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    [SerializeField] int MaxHealth = 100;
    public int currentHealth { get; private set; }
    [SerializeField] protected HealthBar heathBar;

    public Stat Damage;
    public Stat Armor;
    //bool check;
    void Awake()
    {
        currentHealth = MaxHealth;
        heathBar.SetMaxHeathBar(MaxHealth);
    }
    public void TakeDamage(int dmg)
    {
        dmg -= Armor.GetValue();
        Debug.Log(gameObject.name + "dame: " + dmg);
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        currentHealth -= dmg;
        // Debug.Log(transform.name + " takes " + dmg + " damage");
        heathBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Healing(int value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        heathBar.SetHealth(currentHealth);
    }
    public virtual void Die()
    {
        //die someway
        //over
    }
}
