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
    public int currentArmor { get; private set; }
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
        currentArmor = Armor.Value;
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
            Armor.AddEquip(newitem.Defend);
            Damage.AddEquip(newitem.Damage);
        }
        if (oldItem != null) {
            Armor.RemoveEquip(oldItem.Defend);
            Damage.RemoveEquip(oldItem.Damage);
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

        if (currentArmor > 0) {
            int shieldDmg = Mathf.Min(currentArmor, dmg);
            currentArmor -= shieldDmg;
            dmg -= shieldDmg;
            StatPlayerUI?.setShield(currentArmor);
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

        while (currentArmor < GameManager.Instance.LoadedData.armor) {
            yield return new WaitForSeconds(1f);
            currentArmor += 1;
            StatPlayerUI?.setShield(currentArmor);
        }
        StatPlayerUI?.setShield(currentArmor);
        Debug.Log("RegenerateShield");
    }

}
