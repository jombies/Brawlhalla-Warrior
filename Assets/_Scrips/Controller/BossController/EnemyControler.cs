using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
public class EnemyControler : MonoBehaviour
{
    Transform _target;
    NavMeshAgent _agent;
    CharacterAnimation PlayerAnimte;
    PlayerStat _playerStat;
    EnemyStats _enemyStats;
    GameObject canvas;
    //Damage Pop up
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TextMesh _textDamePopup;
    [SerializeField] CapsuleCollider _capsuleCollider;
    //layer
    public LayerMask IsPlayer, IsGround;
    readonly float ac;
    //patrol    
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;

    // Start is called before the first frame update
    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        _target = PlayerReferences.Instance.Player.transform;
        _enemyStats = GetComponent<EnemyStats>();
        _agent = GetComponent<NavMeshAgent>();
        _playerStat = _target.GetComponent<PlayerStat>();
        PlayerAnimte = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
    }
    void Update()
    {
        if (_enemyStats.currentHealth <= 0)
        {
            canvas.SetActive(false);
            _capsuleCollider.enabled = false;
            return;
        }

        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        if (!playerInSight && !playerInAttack) PatrolPlayer();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) Attack();

    }
    void PatrolPlayer()
    {
        if (!walkPointSet)
        {
            _enemyStats.Animator.SetBool("walking", false);
            Invoke(nameof(SearchWalkPoint), 1);
        }
        if (walkPointSet)
        {
            _enemyStats.Animator.SetBool("walking", true);
            _agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walk point reached
        if (distanceToWalkPoint.magnitude < 3f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(walkPoint, out hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable")))
        {
            walkPointSet = true;
        }
        CancelInvoke();
        //if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
        //    walkPointSet = true;
    }
    void ChasePlayer()
    {
        FaceTarget();
        _enemyStats.Animator.SetBool("walking", true);
        _agent.SetDestination(_target.position);
    }
    void Attack()
    {
        FaceTarget();
        if (!_enemyStats.Animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            _enemyStats.Animator.SetBool("walking", false);
            _agent.SetDestination(transform.position);
            // animator.SetTrigger("attack");
        }

        if (!alreadyAttacked)
        {
            _enemyStats.Animator.SetTrigger("attack");
            //animator.SetBool("walking", false);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    void FaceTarget()
    {
        Vector3 fdir = (_target.position - transform.position).normalized;
        Quaternion faceOff = Quaternion.LookRotation(new Vector3(fdir.x, 0, fdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, Time.deltaTime * 10f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weapon") && PlayerAnimte.IsAttacking)
        {
            _textDamePopup.text = (_playerStat.Damage.BaseValue * -1).ToString();
            Instantiate(PopUpDame, transform.position + new Vector3(0, 2.5f, 0), Quaternion.Euler(50, -45, 0));
            _enemyStats.TakeDamage(_playerStat.Damage.BaseValue);
            // Debug.Log("cai :" + other.name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}