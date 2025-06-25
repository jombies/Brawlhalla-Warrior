using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStat : CharacterStat
{
    CharacterAnimation Animator;
    [SerializeField] StatPlayerUI StatPlayerUI;

    [Header("Shield")]
    private Coroutine shieldRegenCoroutine;
    public float shieldRegenDelay = 10f;
    public int currentShield { get; private set; }
    public int level = 0;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<CharacterAnimation>();
        EquipmentManager.Instance.onEquipchanged += OnEquipmentChanged;
        level = GameManager.Instance.LoadedData.level;
        MaxHealth = GameManager.Instance.LoadedData.maxHP;
        Damage.BaseValue = GameManager.Instance.LoadedData.attack;
        Armor.BaseValue = GameManager.Instance.LoadedData.armor;
        currentShield = Armor.Value;
        currentHealth = MaxHealth;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(LoadElement());
    }
    IEnumerator LoadElement()
    {
        yield return null;
        StatPlayerUI = FindAnyObjectByType<StatPlayerUI>();
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
        GameManager.Instance.LoadedData.armor = Armor.Value;
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
                GameManager.Instance.OnPlayerLose();
            }
            AudioManager.Instance.PlaySFX("player hit");
        }

    }

    public void Healing(int value)
    {
        currentHealth += value;
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

        while (currentShield < GameManager.Instance.LoadedData.armor) {
            yield return new WaitForSeconds(1f);
            currentShield += 1;
            StatPlayerUI?.setShield(currentShield);
        }
        StatPlayerUI?.setShield(currentShield);
        Debug.Log("RegenerateShield");
    }

}
