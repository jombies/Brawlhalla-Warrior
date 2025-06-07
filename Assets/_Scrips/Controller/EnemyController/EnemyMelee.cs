using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }
    EnemyState currentState;

    [Header("Components")]
    EnemyController controller;

    [Header("Detection Settings")]
    public LayerMask IsPlayer, IsGround;
    public float attackRange = 2f;
    public float sightRange = 4f;

    [Header("Attack Settings")]
    public float timeBetweenAttacks = 1.5f;
    bool canAttack = true;

    bool hasSetFovAngle;

    NavMeshAgent Agent => controller.Agent;
    Animator Animator => controller.Animator;
    FieldOfView Fov => controller.Fov;

    void Start()
    {
        controller = GetComponent<EnemyController>();
        currentState = EnemyState.Patrol;
    }

    void Update()
    {
        if (controller.IsDead) return;

        UpdateAnimatorSpeed();
        HandleStateLogic();
        HandleStateTransition();
        Agent.isStopped = controller.IsAttack;
    }

    void UpdateAnimatorSpeed()
    {
        float normalizedSpeed = Agent.velocity.magnitude / Agent.speed;
        Animator.SetFloat("Speed", normalizedSpeed);
    }

    void HandleStateLogic()
    {
        switch (currentState) {
            case EnemyState.Patrol:
                DoPatrol();
                break;
            case EnemyState.Chase:
                DoChase();
                break;
            case EnemyState.Attack:
                DoAttack();
                break;
        }
    }

    void HandleStateTransition()
    {
        float distanceToPlayer = controller.Target != null ?
            Vector3.Distance(transform.position, controller.Target.transform.position) : Mathf.Infinity;

        if (!Fov.canSeePlayer) {
            currentState = EnemyState.Patrol;
        }
        else if (distanceToPlayer <= attackRange) {
            currentState = EnemyState.Attack;
        }
        else {
            currentState = EnemyState.Chase;
        }
        if (distanceToPlayer <= sightRange) {
            currentState = EnemyState.Chase;
            if (distanceToPlayer <= attackRange) {
                currentState = EnemyState.Attack;
            }

        }

    }

    void DoPatrol()
    {
        Agent.stoppingDistance = 0;
        Agent.isStopped = false;

        if (Agent.remainingDistance <= Agent.stoppingDistance) {
            if (RandomPoint(transform.position, controller.Fov.radius, out Vector3 point)) {
                Agent.SetDestination(point);
            }
        }
    }

    void DoChase()
    {
        if (!hasSetFovAngle) {
            Fov.angle = 360;
            hasSetFovAngle = true;
        }

        Agent.stoppingDistance = attackRange;
        controller.FaceTarget();
        if (controller.Target != null)
            Agent.SetDestination(controller.Target.transform.position);
    }

    void DoAttack()
    {
        controller.FaceTarget();
        if (!canAttack) return;

        Agent.SetDestination(transform.position); // Đứng tại chỗ

        Animator.SetTrigger("attack");
        canAttack = false;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    public void ApplyDamage()
    {
        if (controller.Target == null) return;

        if (Vector3.Distance(transform.position, controller.Target.transform.position) <= attackRange) {
            var playerStat = controller.Target.GetComponent<PlayerStat>();
            if (playerStat != null) {
                playerStat.TakeDamage(1);
                Debug.Log("Damage!");
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 3.0f, NavMesh.AllAreas)) {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //}
}
