using UnityEngine;

public class CharacterAmintor : MonoBehaviour
{
    Animator animator;
    public bool IsAttacking;
    public float timeSinceAttack;
    public int currentAttack;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        movingAnimte();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void movingAnimte()
    {
        Vector3 direction = new Vector3(InputSingleton.Instance.Horizon, 0, InputSingleton.Instance.Vertical).normalized;
        float ac = Mathf.Clamp01(direction.magnitude) / 2;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ac += ac;
        }
        animator.SetFloat("Velocity", ac, 0.1f, Time.deltaTime);
    }
    void Attack()
    {
        if (timeSinceAttack > 0.8f)
        {
            currentAttack++;
            IsAttacking = true;

            //reset attack
            if (currentAttack > 3) currentAttack = 1;
            if (timeSinceAttack > 1) timeSinceAttack = 1;
            //play animte
            animator.SetTrigger("Attack" + currentAttack);
            timeSinceAttack = 0;
        }


    }
    void NonAttack()
    {
        IsAttacking = false;
    }
}
