using System.Collections;
using UnityEngine;

public class BossAISpawner : MonoBehaviour, IPoolable
{
    EnemyController controller;
    BossVFXHandler vfxHandler;
    enum BossState { Idle, Chase, MeleeAttack, Skill, Recovery, Staggered, Dead }
    [SerializeField] BossState currentState = BossState.Idle;
    public bool isStateExecuting = false;

    [Header("Detection")]
    [SerializeField] Transform Sign;
    [SerializeField] Transform MidPos;
    public LayerMask IsPlayer;



    [Header("Combat Settings")]
    int attacksExecuted = 0;
    private bool canMelee = true;
    public float timeBetweenAttacks;
    private float rotationTime = 90;
    private bool isRotating = false;


    [Header("Skill Settings")]
    [SerializeField] int SkillExecute = 6;
    [SerializeField] GameObject minionPrefab;
    float skillDuration = 3f;           // Thời gian boss thi triển kỹ năng
    float timeBetweenBursts = 0.5f;     // Mỗi lần bắn cách nhau bao lâu
    bool skillOnCooldown = false;
    [SerializeField] float skillCooldown = 15f;

    [Header("recovery Settings")]
    float recoveryTime = 8f;

    //State
    bool hasRecover = false;
    public float attackRange, sightRange;
    public bool PlayerOnGr = false;

    void Start()
    {
        vfxHandler = GetComponent<BossVFXHandler>();
        controller = GetComponent<EnemyController>();
        StartCoroutine(BossStateMachine());
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
                            if (attacksExecuted >= 3 && !skillOnCooldown) {

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

        if (attacksExecuted >= SkillExecute && !skillOnCooldown) {

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

        controller.Agent.isStopped = false;
        controller.Agent.SetDestination(MidPos.position);
        controller.Animator.SetBool("walk", true);


        // wait for the player to reach the mid position
        while (Vector3.Distance(transform.position, MidPos.position) > 1f) {
            yield return null;
        }
        controller.IsAttack = true;
        controller.Agent.isStopped = true;
        controller.Animator.SetBool("walk", false);
        controller.Animator.SetTrigger("skill");

        StartCoroutine(RotateOnpot());
        yield return new WaitForSeconds(10f);

        attacksExecuted = 0;
        controller.IsAttack = false;
        isStateExecuting = false;
        StartCoroutine(SkillCooldownTimer());
        ChangeState(BossState.Chase);
    }
    IEnumerator SkillCooldownTimer()
    {
        yield return new WaitForSeconds(skillCooldown);
        skillOnCooldown = false;
        Debug.Log("Skill cooldown hoàn thành");
    }
    IEnumerator RotateOnpot()
    {
        float elapsed = 0f;
        int angleOffset = 0;
        StartCoroutine(SpawnerMinion());
        while (elapsed < skillDuration) {
            float spinTime = 0f;
            while (spinTime < timeBetweenBursts) {
                transform.Rotate(0f, rotationTime * Time.deltaTime, 0f);
                spinTime += Time.deltaTime;
                yield return null;
            }
            angleOffset += 10;
            elapsed += timeBetweenBursts;
        }
    }
    IEnumerator SpawnerMinion()
    {
        int count = 3;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++) {
            float angle = i * angleStep;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 spawnPosition = transform.position + dir * 3f + Vector3.up * 0.5f;
            //Instantiate(minionPrefab, spawnPosition, Quaternion.LookRotation(-dir));
            GameObject minion = ObjectPoolManager.Instance.Spawn(minionPrefab, spawnPosition, Quaternion.LookRotation(-dir));
            Debug.Log($"Spawned minion at {spawnPosition}");

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ApplyDamage()
    {
        Vector3 toTarget = (controller.Target.transform.position - transform.position);
        float distance = toTarget.magnitude;

        if (distance <= attackRange) {
            float angle = Vector3.Angle(transform.forward, toTarget);

            if (angle <= 30f) // ✅ chỉ tấn công trong 60 độ trước mặt
            {
                var playerStat = controller.Target.GetComponent<PlayerStat>();
                if (playerStat != null) {
                    playerStat.TakeDamage(controller.EnemyStats.Damage.Value);
                    Debug.Log("Boss dealt damage in front cone!");
                }
            }
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
        yield return new WaitForSeconds(2);

        float tick = 0.5f;
        float elapsed = 0f;
        int healPerTick = 3;

        while (elapsed < recoveryTime) {
            //  vfxHandler.PlayRecovery();
            controller.EnemyStats.Heal(healPerTick);
            elapsed += tick;
            yield return new WaitForSeconds(tick);
        }


        // Allow melee attacks again
        hasRecover = true;
        canMelee = true;
        isStateExecuting = false;
        controller.IsAttack = false;
        ChangeState(BossState.Chase);
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

    public void OnSpawnFromPool()
    {
        minionPrefab.GetComponent<EnemyController>().HandleReliveState();
    }

    public void OnReturnToPool()
    {
        throw new System.NotImplementedException();
    }
}
