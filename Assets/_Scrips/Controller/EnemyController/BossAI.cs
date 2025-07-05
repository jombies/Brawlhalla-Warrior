using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    EnemyController controller;
    BossVFXHandler vfx;
    enum BossState { Idle, Chase, MeleeAttack, Skill, Recovery, Staggered, Dead }
    BossState currentState = BossState.Idle;
    public bool isStateExecuting = false;

    [Header("Detection")]
    [SerializeField] Transform Sign;
    [SerializeField] Transform MidPos;
    public LayerMask IsPlayer;
    float StopDistance = 4f;



    [Header("Combat Settings")]
    public int attacksExecuted = 0;
    private bool canMelee = true;
    public float timeBetweenAttacks;
    private float rotationTime = 90;
    private bool isRotating = false;


    [Header("Skill Settings")]
    [SerializeField] int projectileCount;
    [SerializeField] GameObject projectilePrefab;
    float skillDuration = 3f;           // Thời gian boss thi triển kỹ năng
    float timeBetweenBursts = 0.5f;     // Mỗi lần bắn cách nhau bao lâu
    bool skillOnCooldown = false;
    [SerializeField] float skillCooldown = 15f;

    [Header("recovery Settings")]
    float recoveryTime = 8f;

    //State
    bool hasRecover = true;
    public float attackRange, sightRange;
    public bool PlayerOnGr = false;

    void Start()
    {
        ObjectPoolManager.Instance.PreloadPool(projectilePrefab, 30);
        controller = GetComponent<EnemyController>();
        StartCoroutine(BossStateMachine());
        vfx = GetComponent<BossVFXHandler>();
    }
    void Update()
    {
        if (controller.EnemyStats.isDead) return;
        // Look at player when in combat states
        if (currentState == BossState.Chase && !isRotating && !isStateExecuting && !controller.IsAttack) {
            // Đặt điểm đến cho NavMeshAgent
            if (controller.Target != null && controller.Agent != null && !controller.Agent.isStopped) {
                controller.Agent.SetDestination(controller.Target.transform.position);
            }
        }
        controller.Agent.isStopped = controller.IsAttack;
        if (Input.GetKeyDown(KeyCode.F1)) {
            StartCoroutine(ExecuteSkill());
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            StartCoroutine(ExecuteRecoveryState());
        }
    }
    IEnumerator BossStateMachine()
    {
        yield return new WaitForSeconds(0.2f);

        while (true) {
            if (controller.EnemyStats.isDead) { yield return null; continue; }
            switch (currentState) {
                case BossState.Idle:
                    if (PlayerOnGr && !isStateExecuting) {
                        ChangeState(BossState.Chase);
                    }
                    break;

                case BossState.Chase:
                    if (!isStateExecuting && !controller.IsAttack) {
                        ChasePlayer();
                    }
                    if (controller.EnemyStats.currentHealth <= controller.EnemyStats.MaxHealth * 0.5f && !isStateExecuting && !hasRecover) {
                        ChangeState(BossState.Recovery);
                        break;
                    }
                    if (IsPlayerInRange(attackRange)) {
                        if (!PlayerInSight()) {
                            if (!isRotating) {
                                StartCoroutine(RotateTowardsPlayer());
                            }
                        }
                        else if (canMelee && !isRotating) {
                            if (attacksExecuted >= 6 && !skillOnCooldown) {

                                ChangeState(BossState.Skill);
                            }
                            else {
                                ChangeState(BossState.MeleeAttack);
                            }
                        }
                    }
                    break;
                case BossState.MeleeAttack:
                    break;

                case BossState.Skill:
                    break;

                case BossState.Recovery:
                    break;
            }
            yield return new WaitForSeconds(0.1f); // Decision update rate
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
                break;

            case BossState.MeleeAttack:
                controller.Animator.SetBool("walk", false);
                StartCoroutine(ExecuteMeleeAttack());
                break;

            case BossState.Skill:
                StartCoroutine(ExecuteSkill());
                break;

            case BossState.Recovery:
                if ((float)controller.EnemyStats.currentHealth / controller.EnemyStats.MaxHealth < 0.5f) {
                    StartCoroutine(ExecuteRecoveryState());
                }
                else {
                    ChangeState(BossState.Chase);
                }
                break;
        }
    }
    void ChasePlayer()
    {
        controller.Agent.SetDestination(controller.Target.transform.position);
    }
    IEnumerator ExecuteMeleeAttack()
    {
        isStateExecuting = true;
        controller.Agent.isStopped = true;
        controller.Agent.stoppingDistance = attackRange;

        if (PlayerInSight() && IsPlayerInRange(attackRange)) {
            controller.Animator.SetTrigger("attack1");
        }
        else {
            yield return StartCoroutine(RotateTowardsPlayer());

            // Sau khi xoay, kiểm tra lại trước khi tấn công
            if (PlayerInSight() && IsPlayerInRange(attackRange)) {
                controller.Animator.SetTrigger("attack1");

                // Chờ hoàn thành animation tấn công
                yield return new WaitForSeconds(2f);
            }
        }
        yield return new WaitForSeconds(2.5f);
        attacksExecuted++;
        controller.Agent.isStopped = false;
        isStateExecuting = false;

        if (attacksExecuted >= 3 && !skillOnCooldown) {

            ChangeState(BossState.Skill);
        }
        else {
            ChangeState(BossState.Chase);
        }

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

        while (t < 1 && !controller.IsAttack) {
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

    IEnumerator ExecuteSkill()
    {
        controller.Agent.stoppingDistance = 0;
        isStateExecuting = true;
        controller.Agent.isStopped = false;
        controller.Agent.SetDestination(MidPos.position);
        controller.Animator.SetBool("walk", true);

        // wait for the player to reach the mid position
        while (Vector3.Distance(transform.position, MidPos.position) > 1f) {
            yield return null;
        }
        controller.Agent.isStopped = true;
        controller.Animator.SetBool("walk", false);
        controller.Animator.SetTrigger("skill");
        controller.IsAttack = true;
        vfx.PlaySkillCharge();
        float elapsed = 0f;
        int angleOffset = 0;
        while (elapsed < skillDuration) {
            applydamage(angleOffset);
            float spinTime = 0f;
            while (spinTime < timeBetweenBursts) {
                transform.Rotate(0f, rotationTime * Time.deltaTime, 0f);
                spinTime += Time.deltaTime;
                yield return null;
            }
            angleOffset += 10;
            elapsed += timeBetweenBursts;
        }

        attacksExecuted = 0;
        controller.IsAttack = false;
        isStateExecuting = false;
        skillOnCooldown = true;
        StartCoroutine(SkillCooldownTimer());
        ChangeState(BossState.Chase);
    }
    IEnumerator SkillCooldownTimer()
    {
        yield return new WaitForSeconds(skillCooldown);
        skillOnCooldown = false;
        Debug.Log("Skill cooldown hoàn thành");
    }

    void applydamage(int offset)
    {
        float angleStep = 360f / projectileCount;
        for (int i = 0; i < projectileCount; i++) {
            float angle = i * angleStep + offset;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            GameObject bullet = ObjectPoolManager.Instance.Spawn(projectilePrefab, transform.position + dir.normalized * 1f + Vector3.up * 1f, Quaternion.LookRotation(dir));
            bullet.GetComponent<Bossbullet>().damage = controller.EnemyStats.Damage.Value;
        }

    }

    IEnumerator ExecuteRecoveryState()
    {
        controller.Agent.stoppingDistance = 0;
        isStateExecuting = true;
        controller.Agent.isStopped = false;
        controller.Agent.SetDestination(MidPos.position);
        //wait for the player to reach the mid position
        controller.Animator.SetBool("walk", true);
        while (Vector3.Distance(transform.position, MidPos.position) > 1f) {
            yield return null;
        }
        controller.Animator.SetBool("walk", false);
        controller.Agent.isStopped = true;
        controller.Animator.SetTrigger("Recovery");
        controller.IsAttack = true;
        yield return new WaitForSeconds(recoveryTime);

        float tick = 0.5f;
        float elapsed = 0f;
        int healPerTick = 5;

        while (elapsed < recoveryTime) {
            vfx.PlayRecovery();
            controller.EnemyStats.Heal(healPerTick);
            elapsed += tick;
            yield return new WaitForSeconds(tick);
        }
        canMelee = true;
        isStateExecuting = false;
        controller.IsAttack = false;
        ChangeState(BossState.Chase);
    }

    public void ApplyDamage()
    {
        Vector3 toTarget = (controller.Target.transform.position - transform.position);
        float distance = toTarget.magnitude;

        if (distance <= attackRange) {
            float angle = Vector3.Angle(transform.forward, toTarget);

            if (angle <= 50) {
                var playerStat = controller.Target.GetComponent<PlayerStat>();
                if (playerStat != null) {
                    playerStat.TakeDamage(controller.EnemyStats.Damage.Value);
                    AudioManager.Instance.PlaySFX("enemy slash2");
                }
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

    private int field;
}
