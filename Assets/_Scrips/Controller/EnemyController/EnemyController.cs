using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    #region Component References
    GameObject _target;
    public GameObject Target => _target;

    NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    EnemyStats _enemyStats;
    public EnemyStats EnemyStats => _enemyStats;

    FieldOfView _fov;
    public FieldOfView Fov => _fov;
    Animator _animator;
    public Animator Animator => _animator;
    #endregion

    PlayerStat _playerStat;
    CharacterAnimation _playerAnimte;


    #region Combat Settings
    [Header("Combat")]
    public Canvas _canvas;
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TextMeshPro _textDamePopup;
    [SerializeField] float speedRotate = 5;
    public CapsuleCollider _capsuleCollider;
    #endregion

    private static readonly Vector3 PopupOffset = new Vector3(0, 2.5f, 0);
    public bool IsAttack;
    public bool IsDead = false;

    Collider[] col;


    private void Start()
    {
        InitializeComponents();
    }
    void InitializeComponents()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyStats = GetComponent<EnemyStats>();
        _fov = GetComponent<FieldOfView>();
        _canvas = transform.GetChild(0).GetComponent<Canvas>();

        _target = PlayerReferences.Instance.Player;
        if (_target != null) {
            _playerAnimte = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
            _playerStat = _target.GetComponent<PlayerStat>();
        }
    }
    private void FixedUpdate()
    {
        HandleDeathState();
    }

    private void HandleDeathState()
    {
        if (_enemyStats.currentHealth <= 0) {
            IsDead = true;
            if (_capsuleCollider != null) _capsuleCollider.enabled = false;
            if (_canvas != null) _canvas.enabled = false;
            Agent.enabled = false;
        }
    }

    public bool AlreadyFoundPlayer()
    {
        //return _agent.stoppingDistance >= _agent.remainingDistance;
        float distance = Vector3.Distance(_target.transform.position, transform.position);
        if (distance <= _agent.stoppingDistance) return true;
        else return false;

    }

    public void FaceTarget()
    {
        Vector3 fdir = (_target.transform.position - transform.position).normalized;
        Quaternion faceOff = Quaternion.LookRotation(new Vector3(fdir.x, 0, fdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, speedRotate * Time.deltaTime);
    }
    #region attacking event on editer of GameObj
    void IsAttacking() => IsAttack = true;
    void NonAttacking() => IsAttack = false;
    #endregion
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("weapon") && _playerAnimte.IsAttacking) {
            Collider[] colliders = this.GetComponentsInChildren<Collider>(); col = colliders;
            foreach (Collider collider in colliders) {
                // Check if the collider is a hitbox or appropriate target for damage
                if (collider.CompareTag("EnemyHitbox")) {

                    _textDamePopup.text = (_playerStat.Damage.Value * -1).ToString();
                    Instantiate(PopUpDame, transform.position + PopupOffset, Quaternion.identity);
                    _enemyStats.TakeDamage(_playerStat.Damage.Value);
                    Debug.LogWarning(this.gameObject.name);
                    break;
                    //colliders = null;
                }
            }
        }
    }


}