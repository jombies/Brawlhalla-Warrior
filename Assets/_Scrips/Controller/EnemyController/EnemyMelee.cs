using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    [Header("Components")]
    EnemyController enemyController;
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

    NavMeshAgent Agent => enemyController.Agent;
    Animator Animator => enemyController.Animator;
    FieldOfView Fov => enemyController.Fov;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }
    void Update()
    {
        UpdateAnimatorSpeed();
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);


        //if (!Fov.canSeePlayer) {
        //    PatrolPlayer();
        //}
        //else if (!playerInAttack) {
        //    ChasePlayer();
        //}
        //else {
        //    AttackPlayer();
        //}
        if (!Fov.canSeePlayer) {
            PatrolPlayer();
        }
        else {
            ChaseOrAttackPlayer();
        }
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
        if (Agent.remainingDistance <= Agent.stoppingDistance) //done with path
        {
            if (RandomPoint(transform.position, enemyController.Fov.radius, out Vector3 point)) //pass in our centre point and radius of area
            {
                // enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
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

    void ChaseOrAttackPlayer()
    {
        if (!hasSetFovAngle) {
            Fov.angle = 360;
            hasSetFovAngle = true; // Mark as set
        }
        enemyController.FaceTarget();

        if (Agent.remainingDistance > attackRange) {
            ChasePlayer();
        }
        else {
            AttackPlayer();
        }
    }
    void ChasePlayer()
    {
        if (enemyController.IsAttack) return;
        if (!hasSetFovAngle) {
            Fov.angle = 360;
            hasSetFovAngle = true; // Mark as set
        }
        enemyController.FaceTarget();
        Agent.SetDestination(enemyController.Target.transform.position);
    }
    void AttackPlayer()
    {
        Agent.stoppingDistance = attackRange;
        enemyController.FaceTarget(); Agent.SetDestination(enemyController.Target.transform.position);
        if (canAttack) {

            Animator.SetTrigger("attack");
            canAttack = false;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}