using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotting : MonoBehaviour
{
    EnemyController controller;
    //Damage pop up
    public LayerMask IsPlayer, IsGround;
    [SerializeField] GameObject BulletPrefab;
    //patrol
    bool UnderGround = true;
    int pointIndex;
    float Pdistance;
    [SerializeField] Transform pointHolder;
    [SerializeField] List<Transform> points = new();

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
        controller = transform.parent.GetComponent<EnemyController>();
        LoadPoints();
    }

    private void LoadPoints()
    {
        foreach (Transform point in pointHolder)
        {
            points.Add(point);
        }
    }
    void Update()
    {
        if (controller.IsDead) return;
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);

        if (!playerInSight && !playerInAttack) PatrolPlayer();
        if (playerInSight && !playerInAttack) ChasePlayer();
        if (playerInSight && playerInAttack) Attack();
    }
    private void FixedUpdate()
    {
        NextMove();
    }
    void PatrolPlayer()
    {
        UnderGround = true;
        // controller._capsuleCollider.enabled = false;
        controller._canvas.gameObject.SetActive(false);
        controller.Animator.Play("GroundDiveIn");
        attackCount = 0;
    }
    void ChasePlayer()
    {
        controller.FaceTarget();
        if (UnderGround)
        {
            controller.Animator.Play("GroundBreakThrough");
            //controller._capsuleCollider.enabled = true;
            controller._canvas.gameObject.SetActive(true);
            UnderGround = false;
        }
    }
    void Attack()
    {
        if (UnderGround) return;
        controller.FaceTarget();
        if (!alreadyAttacked)
        {
            controller.Animator.SetTrigger("attack");
            GameObject newBullet = Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
            if (newBullet.TryGetComponent<BulletInit>(out var bltd))
            {
                bltd.InitDamage(controller.EnemyStats.Damage.BaseValue);
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
    private void SearchWalkPoint()
    {
        controller.Animator.Play("GroundDiveIn");
        controller._canvas.gameObject.SetActive(false);
        StartCoroutine(GotoNextPoint());
        //  transform.position = Vector3.Lerp(transform.position, currPoint.position, 0.5f);

    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    void NextMove()
    {
        Pdistance = Vector3.Distance(transform.position, CurrentPoint().position);
        if (Pdistance <= 0) pointIndex++;
        if (pointIndex > points.Count) pointIndex = 0;
    }
    Transform CurrentPoint()
    {
        return points[pointIndex];
    }
    IEnumerator GotoNextPoint()
    {
        yield return new WaitForSeconds(1);
        Transform currPoint = CurrentPoint();
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, currPoint.position, 10 * Time.deltaTime);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
