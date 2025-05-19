using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class EnemyStats : CharacterStat
{
    EnemyController controller;
    public event Action OnDeath;
    [SerializeField] HealthBar healthBar;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        healthBar = GetComponentInChildren<HealthBar>();
        if (controller == null) {
            Debug.LogError("EnemyController is missing on " + gameObject.name);
        }
        if (healthBar != null) {
            Debug.LogError("HealthBar is missing on " + gameObject.name);
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
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}