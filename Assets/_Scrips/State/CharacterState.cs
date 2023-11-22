using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;
    bool check;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamege(10);
        }
    }
    public void TakeDamege(int dmg)
    {
        dmg -= armor.GetValue();
        Debug.Log("dame:" + dmg);
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

        currentHealth -= dmg;
        Debug.Log(transform.name + " takes " + dmg + " damage");

        if (currentHealth < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //die someway
        //over
    }
}
