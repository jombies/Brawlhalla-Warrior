using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShotting : MonoBehaviour
{
    Animator animator;
    Transform _target;
    NavMeshAgent _agent;
    CharacterAnimation PlayerAnimate;
    PlayerStat _playerStat;
    EnemyStats _enemyStats;
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TMP_Text _textDamePopup;
    public LayerMask IsPlayer, IsGround;
    [SerializeField] GameObject BulletPrefab;
    //patrol
    bool UnderGround = true;

    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [SerializeField] GameObject FirePoint;

    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;

    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerReferences.Instance.Player.transform;
        animator = GetComponent<Animator>();
        _enemyStats = GetComponent<EnemyStats>();
        _playerStat = _target.GetComponent<PlayerStat>();
        PlayerAnimate = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
    }
    //private void LoadNav()
    //{
    //    GetComponent<NavMeshAgent>().stoppingDistance = 2.2f;
    //}
    void Update()
    {
        if (_enemyStats.currentHealth <= 0) return;
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        if (!playerInSight && !playerInAttack) PatrolPlayer();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) Attack();
    }
    void PatrolPlayer()
    {
        UnderGround = true;
        animator.Play("GroundDiveIn");
    }
    private void SearchWalkPoint()
    {

    }
    void ChasePlayer()
    {
        FaceTarget();
        if (UnderGround)
        {
            animator.Play("GroundBreakThrough");
            UnderGround = false;
        }
    }
    void Attack()
    {
        FaceTarget();
        if (Input.GetKeyDown(KeyCode.O))
        {
            Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
        }


        if (!alreadyAttacked)
        {
            animator.SetTrigger("attack");
            GameObject newBullet = Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
            if (newBullet.TryGetComponent<BulletInit>(out var bltd))
            {
                bltd.InitDamage(_enemyStats.Damage.BaseValue);
            }

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
        if (other.CompareTag("weapon") && PlayerAnimate.IsAttacking)
        {
            _textDamePopup.text = (_playerStat.Damage.BaseValue * -1).ToString();
            Instantiate(PopUpDame, transform.position + new Vector3(0, 2.5f, 0), Quaternion.Euler(50, -45, 0));
            _enemyStats.TakeDamage(_playerStat.Damage.BaseValue);
            Debug.Log("cai :" + other.name);
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
