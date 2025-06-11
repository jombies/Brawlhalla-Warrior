using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shooting : MonoBehaviour
{
    EnemyController enemyController;

    [Header("Patrol Settings")]
    [SerializeField] float range;

    [Header("Attack Settings")]
    [SerializeField] GameObject bulletPrefabs;
    [SerializeField] GameObject firePoint;
    [SerializeField] float attackRange = 7.2f;
    [SerializeField] float stoppingDistance = 7f;
    [SerializeField] float timeBetweenAttacks = 1f;

    bool alreadyAttacked;
    bool hasSetFovAngle;

    NavMeshAgent Agent => enemyController.Agent;
    Animator Animator => enemyController.Animator;
    FieldOfView Fov => enemyController.Fov;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyController.EnemyStats.isDead) return;
        UpdateAnimatorSpeed();

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
        if (Agent.remainingDistance <= Agent.stoppingDistance) {
            if (RandomPoint(this.transform.position, Fov.radius, out Vector3 point)) {

                Agent.SetDestination(point);
            }
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, 1 << NavMesh.GetAreaFromName("Walkable"))) {
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
        Agent.stoppingDistance = 0;
        Agent.SetDestination(enemyController.Target.transform.position);
    }

    void AttackPlayer()
    {
        Agent.stoppingDistance = stoppingDistance;
        Agent.SetDestination(PlayerReferences.Instance.Player.transform.position);
        if (!alreadyAttacked) {
            Animator.SetTrigger("attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void SpawnBullet()
    {
        var newBullet = ObjectPoolManager.Instance.Spawn(bulletPrefabs, firePoint.transform.position, firePoint.transform.rotation);
        newBullet.GetComponent<bulletMove>().speed = 15;
        newBullet.GetComponent<bulletMove>().damage = enemyController.EnemyStats.Damage.Value;
        AudioManager.Instance.PlaySFX("enemy shot");
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
