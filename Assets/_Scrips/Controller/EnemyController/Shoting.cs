using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shoting : MonoBehaviour
{
    EnemyController enemyController;
    [SerializeField] float range;//radius of sphere
    [SerializeField] Transform centrePoint; //centre of the area the agent wants to move around
    //bullet
    [SerializeField] GameObject bulletPrefabs;
    [SerializeField] GameObject firePoint;
    //attack time
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (enemyController.Agent.hasPath)
        //{
        //    enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
        //}
        //else enemyController.Animator.SetFloat("Speed", 0);
        enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude / enemyController.Agent.speed);

        if (!enemyController.fov.canSeePlayer) PatrolPlayer();
        if (enemyController.fov.canSeePlayer) ChasePlayer();
        if (enemyController.fov.canSeePlayer && enemyController.Agent.remainingDistance <= 7.2f) AttackPlayer();
    }
    void PatrolPlayer()
    {
        enemyController.Agent.stoppingDistance = 0;
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
    void ChasePlayer()
    {
        enemyController.fov.angle = 360;
        // enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
        enemyController.FaceTarget();
        enemyController.Agent.SetDestination(enemyController.Target.transform.position);
    }
    void AttackPlayer()
    {
        enemyController.Agent.stoppingDistance = 7;
        enemyController.FaceTarget();
        if (!alreadyAttacked)
        {
            enemyController.Animator.SetTrigger("attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void SpawnBllut()
    {
        GameObject newBullet = Instantiate(bulletPrefabs, firePoint.transform.position, firePoint.transform.rotation);
        if (newBullet.TryGetComponent<BulletInit>(out var bltd))
        {
            bltd.InitDamage(enemyController.EnemyStats.Damage.BaseValue);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
        CancelInvoke();
    }
}
