using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyShotting : MonoBehaviour
{
    //Manager
    Animator animator;
    Transform _target;
    NavMeshAgent _agent;
    CharacterAnimation PlayerAnimate;
    PlayerStat _playerStat;
    EnemyStats _enemyStats;
    //Damage pop up
    [SerializeField] GameObject PopUpDame;
    [SerializeField] Text _textDamePopup;
    public LayerMask IsPlayer, IsGround;
    [SerializeField] GameObject BulletPrefab;
    //patrol
    bool UnderGround = true;
    [SerializeField] BoxCollider BoxCollider;
    int pointIndex;
    float Pdistance;
    public List<Transform> points;
    [SerializeField] Transform currPoint;

    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    int attackCount = 0;
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
        if (_enemyStats.currentHealth <= 0)
        {
            BoxCollider.enabled = false;
            return;
        }
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        if (!playerInSight && !playerInAttack) PatrolPlayer();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) Attack();
    }
    private void FixedUpdate()
    {
        nextMove();
    }
    void PatrolPlayer()
    {
        UnderGround = true;
        animator.Play("GroundDiveIn");
        attackCount = 0;
    }
    private void SearchWalkPoint()
    {
        animator.Play("GroundDiveIn");
        StartCoroutine(enumerator());
        //  transform.position = Vector3.Lerp(transform.position, currPoint.position, 0.5f);

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
        if (UnderGround) return;
        FaceTarget();
        if (!alreadyAttacked)
        {
            animator.SetTrigger("attack");
            GameObject newBullet = Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
            if (newBullet.TryGetComponent<BulletInit>(out var bltd))
            {
                bltd.InitDamage(_enemyStats.Damage.BaseValue);
            }
            alreadyAttacked = true;
            attackCount++;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        if (attackCount == 6)
        {
            SearchWalkPoint();
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
    void nextMove()
    {
        Pdistance = Vector3.Distance(transform.position, currPoint.position);
        if (Pdistance <= 0) pointIndex++;
        if (pointIndex >= points.Count) pointIndex = 0;
        currPoint = points[pointIndex];

    }
    IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1);
        transform.position = Vector3.Lerp(transform.position, currPoint.position, 0.5f);

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
