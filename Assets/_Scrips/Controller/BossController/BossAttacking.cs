using UnityEngine;

public class BossAttacking : MonoBehaviour
{
    BossController controller;
    BossMoving moving;


    //layer
    public LayerMask IsPlayer, IsGround;

    //patrol
    bool isFirst = true;
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
        controller = transform.parent.GetComponent<BossController>();
        moving = GetComponent<BossMoving>();
    }

    // Update is called once per frame
    void Update()
    {
        //SeeFirstTime();
        if (controller.AlreadyFoundPlayer())
        {
            Attacking();
        }
    }
    void Attacking()
    {
        if (controller.IsAttack)
        {
            controller.Animator.SetBool("walking", false);
        }
        if (!alreadyAttacked)
        {
            controller.Animator.SetTrigger("attack1");
            //animator.SetBool("walking", false);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
    bool PlayerInSight()
    {
        return false;
    }
}
