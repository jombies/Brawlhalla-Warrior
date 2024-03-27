using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator _animator;
    public bool IsAttacking;
    public float TimeSinceAttack;
    public int CurrentAttack;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        TimeSinceAttack += Time.deltaTime;
        Attack();
        if (IsAttacking) return;
        MovingAnimte();
    }

    void MovingAnimte()
    {
        Vector3 direction = new Vector3(InputSingleton.instance.horizon, 0, InputSingleton.instance.vertical).normalized;
        float ac = Mathf.Clamp01(direction.magnitude) / 2;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ac += ac;
        }
        _animator.SetFloat("Velocity", ac, 0.1f, Time.deltaTime);
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && TimeSinceAttack > .9f)
        {
            CurrentAttack++;
            //reset attack
            if (CurrentAttack > 3) CurrentAttack = 1;
            if (TimeSinceAttack > 1.1f) CurrentAttack = 1;
            //play animate
            _animator.SetTrigger("Attack" + CurrentAttack);
            TimeSinceAttack = 0;
            IsAttacking = true;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _animator.SetTrigger("power");
            IsAttacking = true;
        }


    }
    public void GetDie()
    {
        _animator.Play("death_idle");
    }
    //triger is attacking at animation character
    void NonAttack() => IsAttacking = false;
}
