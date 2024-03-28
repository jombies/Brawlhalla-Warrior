using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    GameObject _target;
    public GameObject Target => _target;
    NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    EnemyStats _enemyStats;
    public EnemyStats EnemyStats => _enemyStats;
    PlayerStat _playerStat;
    CharacterAnimation _playerAnimte;
    Canvas _canvas;
    public Animator Animator;

    // Damage Popup
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TextMesh _textDamePopup;
    //react
    public bool IsAttack;
    [SerializeField] CapsuleCollider _capsuleCollider;

    void Start()
    {
        Animator = GetComponent<Animator>();
        _target = PlayerReferences.Instance.Player;
        _playerAnimte = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
        _playerStat = _target.GetComponent<PlayerStat>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyStats = GetComponent<EnemyStats>();
        _canvas = transform.GetChild(0).GetComponent<Canvas>();
    }
    private void FixedUpdate()
    {
        if (_enemyStats.currentHealth <= 0)
        {
            Animator.enabled = false;
            _capsuleCollider.enabled = false;
            _canvas.enabled = false;
        }
    }
    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, _target.transform.position);
    }

    public bool AlreadyFoundPlayer()
    {
        return _agent.stoppingDistance >= DistanceToPlayer();
    }
    public void FaceTarget()
    {
        Vector3 fdir = (_target.transform.position - transform.position).normalized;
        Quaternion faceOff = Quaternion.LookRotation(new Vector3(fdir.x, 0, fdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, Time.deltaTime * 5);
    }
    #region attacking event

    void IsAttacking() => IsAttack = true;
    void NonAttacking() => IsAttack = false;

    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weapon") && _playerAnimte.IsAttacking)
        {
            _textDamePopup.text = (_playerStat.Damage.BaseValue * -1).ToString();
            Instantiate(PopUpDame, transform.position + new Vector3(0, 2.5f, 0), Quaternion.Euler(50, -45, 0));
            _enemyStats.TakeDamage(_playerStat.Damage.BaseValue);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3);
    }
}
