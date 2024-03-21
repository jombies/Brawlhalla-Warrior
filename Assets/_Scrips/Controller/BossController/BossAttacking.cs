using UnityEngine;
using UnityEngine.AI;

public class BossAttacking : MonoBehaviour
{
    GameObject _target;
    CharacterAnimation _playerAnimte;
    PlayerStat _playerStat;
    NavMeshAgent _agent;
    EnemyStats _enemyStats;

    //layer
    public LayerMask IsPlayer, IsGround;

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
        _target = PlayerReferences.Instance.Player;
        _playerAnimte = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
        _playerStat = _target.GetComponent<PlayerStat>();
        _agent = transform.parent.GetComponent<NavMeshAgent>();
        _enemyStats = transform.parent.GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
