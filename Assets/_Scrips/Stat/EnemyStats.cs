using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
public class EnemyStats : CharacterStat
{
    Animator animator;
    public Animator Animator => animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        heathBar = GetComponentInChildren<HealthBar>();
    }
    public override void Die()
    {
        base.Die();
        Debug.Log(this.gameObject.name + " Died");
        PlayDead();
    }

    void PlayDead()
    {
        animator.SetTrigger("Dead");
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