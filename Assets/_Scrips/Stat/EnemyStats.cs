using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyHitFlasht))]
public class EnemyStats : CharacterStat
{
    EnemyController controller;
    EnemyHitFlasht flasht;
    public event Action OnDeath;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        flasht = GetComponent<EnemyHitFlasht>();
        controller = GetComponent<EnemyController>();
        healthBar = GetComponentInChildren<HealthBar>();
        if (controller == null) {
            Debug.LogError("EnemyController is missing on " + gameObject.name);
        }
        if (healthBar != null) {
            healthBar.SetMaxHeathBar(MaxHealth);
        }
    }
    protected override void OnTakeDamage(int dmg)
    {
        if (dmg > 0) {
            currentHealth -= dmg;
            healthBar?.SetHealth(currentHealth);
            if (currentHealth <= 0 && !isDead) {
                Die();
                isDead = true;
            }
            flasht.Flash();
        }
    }

    protected override void Die()
    {
        base.Die();
        OnDeath?.Invoke();
        PlayDead();
    }

    void PlayDead()
    {
        controller.Animator.SetTrigger("Dead");

        //this.StartCoroutine(WaitToDie(animationTime + 1));
        StartCoroutine(WaitToDie(3));
    }
    IEnumerator WaitToDie(float s)
    {
        //Debug.Log($"{s}");
        yield return new WaitForSeconds(s);
        var pooled = GetComponent<PooledObject>();
        if (pooled != null) {
            pooled.ReturnToPool();
        }
        else {
            gameObject.SetActive(false);
        }

        //Destroy(gameObject);
    }
    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
        healthBar?.SetHealth(currentHealth);
    }
    public void ResetStat()
    {
        currentHealth = MaxHealth;
        isDead = false;
        healthBar?.SetMaxHeathBar(MaxHealth);
    }
}