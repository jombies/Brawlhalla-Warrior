using System.Collections;
using UnityEngine;

public class EnemyShotting : MonoBehaviour
{
    EnemyController controller;

    [Header("Bắn đạn")]
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] GameObject FirePoint;

    [Header("Player Detection")]
    public LayerMask IsPlayer;
    public float attackRange;
    public float sightRange;

    [Header("Tấn công")]
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked = false;

    // Trạng thái
    bool playerInSight;
    bool playerInAttack;
    bool isUnderground = true;

    void Start()
    {
        controller = transform.parent.GetComponent<EnemyController>();
    }

    void Update()
    {
        if (controller.EnemyStats.isDead) return;

        // Kiểm tra vùng nhìn thấy và tấn công
        playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        if (playerInSight) {
            RiseFromGround(); // nổi lên nếu thấy người chơi

            if (playerInAttack) {
                Attack(); // bắn nếu đủ gần
            }
        }
        else {
            DiveIntoGround(); // không thấy player thì lặn xuống
        }
        if (playerInSight && !isUnderground) {
            controller.FaceTarget();
        }
    }

    void RiseFromGround()
    {
        if (isUnderground) {
            controller.Animator.Play("GroundBreakThrough"); // animation trồi lên
            controller._canvas.gameObject.SetActive(true);  // hiện UI máu nếu có
            isUnderground = false;
        }
    }

    void DiveIntoGround()
    {
        if (!isUnderground) {
            controller.Animator.Play("GroundDiveIn"); // animation lặn xuống
            controller._canvas.gameObject.SetActive(false); // ẩn UI máu nếu có
            isUnderground = true;
        }
    }

    void Attack()
    {
        if (isUnderground || alreadyAttacked) return;

        controller.FaceTarget();
        controller.Animator.SetTrigger("attack");

        GameObject newBullet = Instantiate(BulletPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
        newBullet.GetComponent<bulletMove>().damage = controller.EnemyStats.Damage.Value;
        AudioManager.Instance.PlaySFX("enemy shot");

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
