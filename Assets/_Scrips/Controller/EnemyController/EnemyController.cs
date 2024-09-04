using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameObject _target;
    public GameObject Target => _target;
    NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    EnemyStats _enemyStats;
    public EnemyStats EnemyStats => _enemyStats;
    PlayerStat _playerStat;
    CharacterAnimation _playerAnimte;
    public Canvas _canvas;
    public Animator Animator;
    GameObject _camera;
    Collider[] col;
    // Damage Popup
    [SerializeField] GameObject PopUpDame;
    [SerializeField] TextMeshPro _textDamePopup;
    //react
    [SerializeField] float speedRotate = 5;
    public bool IsAttack;
    public bool IsDead = false;
    public CapsuleCollider _capsuleCollider;

    void Start()
    {
        Animator = GetComponent<Animator>();
        _target = PlayerReferences.Instance.Player;
        _playerAnimte = _target.transform.GetChild(0).GetComponent<CharacterAnimation>();
        _playerStat = _target.GetComponent<PlayerStat>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyStats = GetComponent<EnemyStats>();
        _canvas = transform.GetChild(0).GetComponent<Canvas>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void FixUpdate()
    {
        if (_enemyStats.currentHealth == 0)
        {
            _capsuleCollider.enabled = false;
            _canvas.enabled = false;
            IsDead = true;
        }
    }
    public bool AlreadyFoundPlayer()
    {
        return _agent.stoppingDistance >= _agent.remainingDistance;
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
        if (other.CompareTag("weapon") && _playerAnimte.IsAttacking)
        {
            Collider[] colliders = this.GetComponentsInChildren<Collider>(); col = colliders;
            foreach (Collider collider in colliders)
            {
                // Check if the collider is a hitbox or appropriate target for damage
                if (collider.CompareTag("EnemyHitbox"))
                {

                    _textDamePopup.text = (_playerStat.Damage.BaseValue * -1).ToString();
                    Instantiate(PopUpDame, transform.position + new Vector3(0, 2.5f, 0), Quaternion.identity);
                    _enemyStats.TakeDamage(_playerStat.Damage.BaseValue);
                    Debug.LogError(this.gameObject.name);
                    //colliders = null;
                }
            }
        }
        //private void OnDrawGizmosSelected()
        //{
        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireSphere(transform.position, 3);
        //}
    }
}