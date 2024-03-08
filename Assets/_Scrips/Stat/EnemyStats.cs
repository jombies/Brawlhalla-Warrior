using System.Collections;
using UnityEngine;

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
        StartCoroutine(WaitToDie(animator.runtimeAnimatorController.animationClips.Length));
    }

    void PlayDead()
    {
        animator.SetTrigger("Dead");
    }
    IEnumerator WaitToDie(float s)
    {
        Debug.Log($"{s}");
        yield return new WaitForSeconds(s);
        Destroy(gameObject);
    }
}