using UnityEngine;

public class BossAttacking : MonoBehaviour
{
    EnemyController controller;
    BossMoving moving;
    //layer
    public LayerMask IsPlayer, IsGround;

    //patrol
    bool isFirst = true;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public Vector3 direction; // Hướng của raycast

    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //State
    public float attackRange, sightRange;
    bool playerInSight, playerInAttack;


    // Start is called before the first frame update
    void Start()
    {
        controller = transform.parent.GetComponent<EnemyController>();
        moving = GetComponent<BossMoving>();
    }

    // Update is called once per frame
    void Update()
    {
        //SeeFirstTime();
        if (controller.AlreadyFoundPlayer())
        {
            if (PlayerInSight())
            {
                Attacking();
            }
        }
    }

    private void LateUpdate()
    {
        if (controller.AlreadyFoundPlayer())
        {
            if (!PlayerInSight() && !controller.IsAttack)
            {
                controller.FaceTarget();
            }
        }
    }
    void Attacking()
    {
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
    //Kiem tra player co o truoc mat ko
    bool PlayerInSight()
    {
        Ray ray = new(transform.position, transform.parent.TransformDirection(Vector3.forward));
        RaycastHit hit;
        // Kiểm tra xem ray có va chạm với bất kỳ đối tượng nào không
        if (Physics.Raycast(ray, out hit, 3, IsPlayer))
        {
            // Nếu ray va chạm với một đối tượng, vẽ đường từ điểm bắt đầu đến điểm va chạm
            Debug.DrawLine(transform.position, hit.point, Color.red);
            return true;
        }
        else
        {
            // Nếu ray không va chạm với bất kỳ đối tượng nào, vẽ đường từ điểm bắt đầu theo hướng đã cho
            Debug.DrawRay(transform.position, transform.parent.TransformDirection(Vector3.forward) * 3, Color.green);
            return false;
        }
    }
}
