using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    EnemyController controller;
    //layer
    public LayerMask IsPlayer, IsGround;
    //patrol    
    Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyController>();
    }
    void Update()
    {
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        if (controller.IsDead && controller.IsAttack) return;
        if (!playerInSight && !playerInAttack) PatrolPlayer();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) Attack();
    }
    void PatrolPlayer()
    {
        if (!walkPointSet)
        {
            controller.EnemyStats.Animator.SetBool("walking", false);
            Invoke(nameof(SearchWalkPoint), 1);
        }
        if (walkPointSet)
        {
            controller.EnemyStats.Animator.SetBool("walking", true);
            controller.Agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walk point reached
        if (distanceToWalkPoint.magnitude < 3f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkPoint, out hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            walkPointSet = true;
        }
        CancelInvoke();
        //if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
        //    walkPointSet = true;
    }
    void ChasePlayer()
    {
        if (controller.IsAttack) return;
        controller.FaceTarget();
        controller.Agent.isStopped = false;
        controller.EnemyStats.Animator.SetBool("walking", true);
        controller.Agent.SetDestination(controller.Target.transform.position);
    }
    void Attack()
    {
        controller.FaceTarget();
        if (!controller.EnemyStats.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            controller.EnemyStats.Animator.SetBool("walking", false);
            controller.Agent.SetDestination(controller.Target.transform.position);
            controller.Agent.isStopped = true;
            // animator.SetTrigger("attack");
        }

        if (!alreadyAttacked)
        {
            controller.EnemyStats.Animator.SetTrigger("attack");
            //animator.SetBool("walking", false);

            alreadyAttacked = true;
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