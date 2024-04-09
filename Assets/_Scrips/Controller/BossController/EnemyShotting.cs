using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public List<Transform> points;


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
        controller.Animator.Play("GroundDiveIn");
        attackCount = 0;
    }
    private void SearchWalkPoint()
    {
        controller.Animator.Play("GroundDiveIn");
        StartCoroutine(GotoNextPoint());
        //  transform.position = Vector3.Lerp(transform.position, currPoint.position, 0.5f);

    }
    void ChasePlayer()
    {
        controller.FaceTarget();
        if (UnderGround)
        {
            controller.Animator.Play("GroundBreakThrough");
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
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    void NextMove()
    {
        Pdistance = Vector3.Distance(transform.position, CurrentPoint().position);
        if (Pdistance <= 0) pointIndex++;
        if (pointIndex >= points.Count) pointIndex = 0;
    }
    Transform CurrentPoint()
    {
        return points[pointIndex];
    }
    IEnumerator GotoNextPoint()
    {
        yield return new WaitForSeconds(1);
        transform.position = Vector3.Lerp(transform.position, CurrentPoint().position, 10 * Time.deltaTime);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
