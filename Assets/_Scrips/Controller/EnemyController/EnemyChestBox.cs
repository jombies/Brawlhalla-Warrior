using UnityEngine;

public class EnemyChestBox : MonoBehaviour
{
    EnemyController controller;
    //layer
    public LayerMask IsPlayer, IsGround;
    //attack
    bool isAttack = false;
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;
    bool IsAwake = false, hasRun = false;

    void Start()
    {
        controller = transform.parent.GetComponent<EnemyController>();
    }
    void Update()
    {
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        WakeUp();
        if (!playerInAttack && IsAwake && !controller.IsAttack) ChasePlayer();
        if (controller.AlreadyFoundPlayer() && IsAwake) Attack();
        if (controller.IsDead) Dead();
    }

    private void Dead()
    {
        controller.Agent.SetDestination(transform.position);
    }

    public void WakeUp()
    {
        if (playerInAttack && !hasRun)
        {
            controller.Animator.SetBool("WakeUp", true);
            IsAwake = true;
            hasRun = false;
        }
        if (IsAwake)
        {
            controller._canvas.gameObject.SetActive(true);
        }
        else controller._canvas.gameObject.SetActive(false);
    }

    private void ChasePlayer()
    {
        controller.FaceTarget();
        controller.Agent.SetDestination(controller.Target.transform.position);
        controller.Animator.SetBool("walk", true);
    }

    private void Attack()
    {
        controller.Animator.SetBool("walk", false);
        controller.Agent.SetDestination(transform.position);
        if (controller.IsAttack == false)
        {
            controller.FaceTarget();
        }
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            controller.Animator.SetTrigger("attack1");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
