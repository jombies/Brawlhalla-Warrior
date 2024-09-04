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
    void Start()
    {
        controller = GetComponent<EnemyController>();
        heathBar = GetComponentInChildren<HealthBar>();
    }
    public override void Die()
    {
        // base.Die();
        OnDeath?.Invoke();
        Debug.Log(this.gameObject.name + " Died");
        PlayDead();
    }

    void PlayDead()
    {
        controller.Animator.SetTrigger("Dead");
        //AnimatorStateInfo State = this.animator.GetCurrentAnimatorStateInfo(0);
        //float animationTime = State.normalizedTime * State.length;
        //Debug.Log(animationTime);
        //this.StartCoroutine(WaitToDie(animationTime + 1));
        this.StartCoroutine(WaitToDie(1.5f));
    }
    IEnumerator WaitToDie(float s)
    {
        //Debug.Log($"{s}");
        yield return new WaitForSeconds(s);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}