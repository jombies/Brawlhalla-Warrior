using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    EnemyController controller;
    enum BossState { Idle, Chase, MeleeAttack, Skill, Recovery, Staggered, Dead }
    BossState currentState = BossState.Idle;
    public bool isStateExecuting = false;

    [Header("Detection")]
    public LayerMask IsPlayer;
    float StopDistance = 4f;
    [SerializeField] Transform Sign;


    [Header("Combat Settings")]
    private bool canMelee = true;
    public float timeBetweenAttacks;
    private float rotationTime = 1f;
    private bool isRotating = false;
    bool alreadyAttacked;


    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;
    public bool isfirst = true;
    public bool PlayerOnGr = false;


    void Start()
    {
        controller = GetComponent<EnemyController>();
        StartCoroutine(BossStateMachine());
    }
    void Update()
    {
        if (controller.IsDead) return;
        // Look at player when in combat states
        if (currentState == BossState.Chase && !isRotating && !isStateExecuting && !controller.IsAttack) {
            // Đặt điểm đến cho NavMeshAgent
            if (controller.Target != null && controller.Agent != null && !controller.Agent.isStopped) {
                controller.Agent.SetDestination(controller.Target.transform.position);
            }
        }

    }
    IEnumerator BossStateMachine()
    {
        yield return new WaitForSeconds(0.3f);

        while (true) {
            if (controller.IsDead) { yield return null; continue; }
            switch (currentState) {
                case BossState.Idle:
                    if (PlayerOnGr && !isStateExecuting) {
                        ChangeState(BossState.Chase);
                    }
                    break;

                case BossState.Chase:
                    if (/*!isStateExecuting &&*/ !controller.IsAttack) {
                        ChasePlayer();
                    }
                    if (IsPlayerInRange(attackRange)) {
                        if (!PlayerInSight()) {
                            if (!isRotating) {
                                StartCoroutine(RotateTowardsPlayer());
                            }
                        }
                        else if (canMelee && !isRotating) {
                            ChangeState(BossState.MeleeAttack);
                        }
                    }
                    //else if (IsPlayerInRange(specialAttackRange) && !IsPlayerInRange(attackRange) && canUseSpecial && specialAttackTimer <= 0) {
                    //    ChangeState(BossState.SpecialAttack);
                    //}
                    break;

                case BossState.MeleeAttack:
                    //if (IsPlayerInRange(attackRange) && !PlayerInSight() && !isRotating) {
                    //    StartCoroutine(RotateTowardsPlayer());
                    //}

                    break;

                case BossState.Skill:
                    // Special attack behavior is handled in Execute state coroutines
                    break;

                case BossState.Recovery:
                    // Recovery behavior is handled in Execute state coroutines
                    break;

                case BossState.Staggered:
                    // Staggered behavior is handled in Execute state coroutines
                    break;
            }

            //// Special attack cooldown timer
            //if (specialAttackTimer > 0) {
            //    specialAttackTimer -= 0.1f;
            //}

            yield return new WaitForSeconds(0.2f); // Decision update rate
        }
    }
    void ChangeState(BossState newState)
    {
        if (currentState == BossState.MeleeAttack && isStateExecuting) return;

        currentState = newState;
        switch (newState) {
            case BossState.Idle:
                controller.Agent.isStopped = true;
                break;

            case BossState.Chase:
                controller.Agent.isStopped = false;
                if (!controller.Animator.GetBool("walk")) {
                    controller.Animator.SetBool("walk", true);
                }
                //controller.Agent.speed = approachSpeed;
                break;

            case BossState.MeleeAttack:
                controller.Animator.SetBool("walk", false);
                StartCoroutine(ExecuteMeleeAttack());
                break;

                //case BossState.Skill:
                //    StartCoroutine(ExecuteSpecialAttack());
                //    break;

                //case BossState.Recovery:
                //    StartCoroutine(ExecuteRecoveryState());
                //    break;

                //case BossState.Staggered:
                //    StartCoroutine(ExecuteStaggeredState());
                //    break;

                //case BossState.Enraged:
                //    // Handled in TriggerEnragedState method
                //    break;
        }
    }

    IEnumerator ExecuteMeleeAttack()
    {
        isStateExecuting = true;
        controller.Agent.isStopped = true;
        controller.Agent.stoppingDistance = StopDistance;

        if (PlayerInSight() && IsPlayerInRange(attackRange)) {
            controller.Animator.SetTrigger("attack1");
        }
        else {
            // Xoay người đối mặt với người chơi
            yield return StartCoroutine(RotateTowardsPlayer());

            // Sau khi xoay, kiểm tra lại trước khi tấn công
            if (PlayerInSight() && IsPlayerInRange(attackRange)) {
                controller.Animator.SetTrigger("attack1");

                // Chờ hoàn thành animation tấn công
                yield return new WaitForSeconds(2f);
            }
            else {
                // Nếu người chơi đã di chuyển ra khỏi tầm nhìn hoặc tầm tấn công, quay lại chase
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return new WaitForSeconds(2f);
        controller.Agent.isStopped = false;
        isStateExecuting = false;

        ChangeState(BossState.Chase);

    }
    void ChasePlayer()
    {
        //controller.Agent.stoppingDistance = 0;
        controller.Agent.isStopped = false;
        //  controller.Agent.SetDestination(controller.Target.transform.position);

    }
    IEnumerator RotateTowardsPlayer()
    {
        if (controller.IsAttack) yield break;

        isRotating = true;
        controller.Agent.isStopped = true;
        controller.Animator.SetBool("walk", false);

        // Rotate towards player with simple lerp
        Vector3 direction = controller.Target.transform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float rotateTime = 0.3f;
        float t = 0;

        while (t < 1) {
            if (controller.IsAttack) break;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            t += Time.deltaTime / rotateTime;
            yield return null;
        }

        isRotating = false;

        if (currentState == BossState.Chase) {
            controller.Agent.isStopped = false;
            controller.Animator.SetBool("walk", true);

            if (PlayerInSight() && IsPlayerInRange(attackRange) && canMelee) {
                ChangeState(BossState.MeleeAttack);
            }
        }
    }

    bool IsPlayerInRange(float range)
    {
        if (controller.Target == null) return false;

        return Vector3.Distance(transform.position, controller.Target.transform.position) <= range;
    }
    bool PlayerInSight()
    {
        Vector3 direction = Sign.forward;
        Ray ray = new(Sign.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, IsPlayer)) {
            Debug.DrawLine(Sign.position, hit.point, Color.red);
            return true;
        }
        else {
            Debug.DrawRay(Sign.position, direction * attackRange, Color.green);
            return false;
        }
    }
}
