using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [Header("Components")]
    EnemyController controller;
    //layer
    [Header("Detection Settings")]
    public LayerMask IsPlayer, IsGround;
    public float attackRange, sightRange;

    [Header("Attack Settings")]
    public float timeBetweenAttacks;
    bool canAttack = true;

    //State
    bool playerInSight, playerInAttack;
    bool hasSetFovAngle;

    NavMeshAgent Agent => controller.Agent;
    Animator Animator => controller.Animator;
    FieldOfView Fov => controller.Fov;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyController>();
    }
    void Update()
    {
        if (controller.IsDead) return;
        UpdateAnimatorSpeed();
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);


        if (!Fov.canSeePlayer) {
            PatrolPlayer();
        }
        else {
            ChasePlayer();
        }
        if (Fov.canSeePlayer && playerInAttack) {
            AttackPlayer();
        }
        Agent.isStopped = controller.IsAttack;
    }

    void UpdateAnimatorSpeed()
    {
        float normalizedSpeed = Agent.velocity.magnitude / Agent.speed;
        Animator.SetFloat("Speed", normalizedSpeed);
    }

    void PatrolPlayer()
    {
        Agent.stoppingDistance = 0;
        Agent.isStopped = false;
        if (Agent.remainingDistance <= Agent.stoppingDistance) {
            if (RandomPoint(transform.position, controller.Fov.radius, out Vector3 point)) {
                Agent.SetDestination(point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 3.0f, 1 << NavMesh.GetAreaFromName("Walkable"))) {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    void ChasePlayer()
    {
        if (!hasSetFovAngle) {
            Fov.angle = 360;
            hasSetFovAngle = true;
        }
        // if (controller.IsAttack) return;
        Agent.stoppingDistance = attackRange;
        controller.FaceTarget();
        Agent.SetDestination(controller.Target.transform.position);
    }
    void AttackPlayer()
    {
        controller.FaceTarget();
        //Agent.isStopped = true;
        Agent.SetDestination(controller.Target.transform.position);
        if (canAttack) {

            Animator.SetTrigger("attack");
            canAttack = false;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        canAttack = true;
        //Agent.isStopped = false;
    }

    public void ApplyDamage()
    {
        float range = attackRange;
        if (Vector3.Distance(transform.position, controller.Target.transform.position) <= range) {
            var PlayerStat = controller.Target.transform.GetComponent<PlayerStat>();
            if (PlayerStat != null) {
                PlayerStat.TakeDamage(1);
                Debug.Log("Damage!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}