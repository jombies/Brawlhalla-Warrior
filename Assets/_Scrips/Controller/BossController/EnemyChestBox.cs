using UnityEngine;

public class EnemyChestBox : MonoBehaviour
{
    EnemyController controller;
    //layer
    public LayerMask IsPlayer, IsGround;
    //attack
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
        if (!playerInAttack && IsAwake) ChasePlayer();
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
        controller.Agent.SetDestination(transform.position);
        controller.Animator.SetBool("walk", false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
