using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BossMoving : MonoBehaviour
{
    GameObject _target;
    NavMeshAgent _agent;
    EnemyStats _enemyStats;

    [SerializeField] GameObject PopUpDame;
    [SerializeField] TMP_Text _textDamePopup;

    //react
    bool isfirst = true;
    public bool PlayerOnGr = false;

    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerReferences.Instance.Player;
        _agent = transform.parent.GetComponent<NavMeshAgent>();
        _enemyStats = transform.parent.GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerOnGr)
        {
            if (!_enemyStats.Animator.enabled) { _enemyStats.Animator.enabled = true; }
            if (!AlreadyFoundPlayer())
            {
                GetMoving();
            }
            if (AlreadyFoundPlayer())
            {
                StopWalking();
            }
        }
        else if (_enemyStats.Animator.enabled) { _enemyStats.Animator.enabled = false; }
    }
    public void GetMoving()
    {
        FaceTarget();
        _agent.SetDestination(_target.transform.position);
        _enemyStats.Animator.SetBool("walk", true);
    }
    public void StopWalking()
    {
        _enemyStats.Animator.SetBool("walk", false);
        if (isfirst)
        {
            _enemyStats.Animator.SetTrigger("taunting");
            isfirst = false;
            return;
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


    void FaceTarget()
    {
        Vector3 fdir = (_target.transform.position - transform.position).normalized;
        Quaternion faceOff = Quaternion.LookRotation(new Vector3(fdir.x, 0, fdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, Time.deltaTime * 10f);
    }


}
