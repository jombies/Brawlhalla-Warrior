using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    EnemyController enemyController;
    //layer
    public LayerMask IsPlayer, IsGround;
    //patrol    
    [SerializeField] float range;//radius of sphere
    Transform centrePoint; //centre of the area the agent wants to move around
    //Vector3 walkPoint;
    //bool walkPointSet;
    //public float walkPointRange;
    //attack
    public float timeBetweenAttacks;
    [SerializeField] bool alreadyAttacked;

    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        centrePoint = this.transform;
    }
    void Update()
    {
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        //playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        //if (enemyController.IsDead && enemyController.IsAttack) return;
        //if (!playerInSight && !playerInAttack) PatrolPlayer();
        //if (playerInSight && !playerInAttack) ChasePlayer();
        //if (playerInSight && playerInAttack) Attack();

        if (!enemyController.fov.canSeePlayer) PatrolPlayer();
        if (enemyController.fov.canSeePlayer) ChasePlayer();
        //if (enemyController.fov.canSeePlayer && enemyController.Agent.remainingDistance <= 2) AttackPlayer();
        if (enemyController.fov.canSeePlayer && playerInAttack) AttackPlayer();
    }

    void PatrolPlayer()
    {
        enemyController.Agent.stoppingDistance = 0;
        enemyController.Agent.isStopped = false;
        if (enemyController.Agent.remainingDistance <= enemyController.Agent.stoppingDistance) //done with path
        {
            //enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
            if (RandomPoint(centrePoint.position, enemyController.fov.radius, out Vector3 point)) //pass in our centre point and radius of area
            {
                // enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
                enemyController.Agent.SetDestination(point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
    //void PatrolPlayer()
    //{
    //    controller.Agent.isStopped = false;
    //    if (!walkPointSet)
    //    {
    //        controller.Animator.SetBool("walking", false);
    //        Invoke(nameof(SearchWalkPoint), 1);
    //    }
    //    if (walkPointSet)
    //    {
    //        controller.Animator.SetBool("walking", true);
    //        controller.Agent.SetDestination(walkPoint);
    //    }
    //    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //    //Walk point reached
    //    if (distanceToWalkPoint.magnitude < 3f)
    //        walkPointSet = false;
    //}
    //private void SearchWalkPoint()
    //{
    //    //Calculate random point in range
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    //    NavMeshHit hit;
    //    if (NavMesh.SamplePosition(walkPoint, out hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable")))
    //    {
    //        walkPointSet = true;
    //    }
    //    CancelInvoke();
    //    //if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
    //    //    walkPointSet = true;
    //}
    void ChasePlayer()
    {
        if (enemyController.IsAttack) return;
        enemyController.fov.angle = 360;
        enemyController.FaceTarget();
        enemyController.Agent.isStopped = false;
        enemyController.Animator.SetBool("walking", true);
        enemyController.Agent.SetDestination(enemyController.Target.transform.position);
    }
    void AttackPlayer()
    {
        enemyController.Agent.stoppingDistance = 2.5f;
        enemyController.FaceTarget();
        if (!enemyController.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            enemyController.Animator.SetBool("walking", false);
            enemyController.Agent.SetDestination(enemyController.Target.transform.position);
            enemyController.Agent.isStopped = true;
            // animator.SetTrigger("attack");
        }

        if (!alreadyAttacked)
        {
            enemyController.Animator.SetTrigger("attack");
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
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}