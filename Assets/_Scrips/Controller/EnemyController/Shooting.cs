using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    EnemyController enemyController;

    [Header("Patrol Settings")]
    [SerializeField] float range;//radius of sphere
    [SerializeField] Transform centrePoint; //centre of the area the agent wants to move around

    [Header("Attack Settings")]
    [SerializeField] GameObject bulletPrefabs;
    [SerializeField] GameObject firePoint;
    [SerializeField] float attackRange = 7.2f; // Distance to start attacking
    [SerializeField] float stoppingDistance = 7f; // Distance to stop moving while attacking
    [SerializeField] float timeBetweenAttacks = 1f; // Time between attacks

    bool alreadyAttacked;
    bool hasSetFovAngle;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimatorSpeed();

        if (!enemyController.fov.canSeePlayer)
        {
            PatrolPlayer();
        }
        else
        {
            ChaseOrAttackPlayer();
        }
    }

    void UpdateAnimatorSpeed()
    {
        float normalizedSpeed = enemyController.Agent.velocity.magnitude / enemyController.Agent.speed;
        enemyController.Animator.SetFloat("Speed", normalizedSpeed);
    }
    void PatrolPlayer()
    {
        enemyController.Agent.stoppingDistance = 0;
        if (enemyController.Agent.remainingDistance <= enemyController.Agent.stoppingDistance)
        {
            //enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
            if (RandomPoint(centrePoint.position, enemyController.fov.radius, out Vector3 point))
            {
                // enemyController.Animator.SetFloat("Speed", enemyController.Agent.velocity.magnitude);
                enemyController.Agent.SetDestination(point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
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
        if (!hasSetFovAngle)
        {
            enemyController.fov.angle = 360;
            hasSetFovAngle = true; // Mark as set
        }
        enemyController.FaceTarget();

        if (enemyController.Agent.remainingDistance > attackRange)
        {
            ChasePlayer();
        }
        else
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        enemyController.Agent.stoppingDistance = 0;
        enemyController.Agent.SetDestination(enemyController.Target.transform.position);
    }

    void AttackPlayer()
    {
        enemyController.Agent.stoppingDistance = stoppingDistance;
        enemyController.Agent.SetDestination(enemyController.Target.transform.position);
        if (!alreadyAttacked)
        {
            enemyController.Animator.SetTrigger("attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void SpawnBullet()
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
    }
}
