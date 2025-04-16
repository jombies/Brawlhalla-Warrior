using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator _animator;
    AutoAimForPlayer _aim;
    public bool IsAttacking;
    public float TimeSinceAttack;
    public int CurrentAttack;

    private static readonly int VelocityHash = Animator.StringToHash("Velocity");
    private static readonly int PowerHash = Animator.StringToHash("power");
    private static readonly int[] AttackHashes = {
        Animator.StringToHash("Attack1"),
        Animator.StringToHash("Attack2"),
        Animator.StringToHash("Attack3")
    };

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _aim = transform.parent.GetComponent<AutoAimForPlayer>();
    }
    private void Update()
    {
        TimeSinceAttack += Time.deltaTime;

        if (IsAttacking) return;
        Attack();
        MovingAnimte();
    }

    void MovingAnimte()
    {
        Vector3 direction = new Vector3(InputSingleton.instance.horizon, 0, InputSingleton.instance.vertical).normalized;
        float MovementSpeed = Mathf.Clamp01(direction.magnitude) / 2;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            MovementSpeed += MovementSpeed;
        }
        _animator.SetFloat(VelocityHash, MovementSpeed, 0.1f, Time.deltaTime);
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && TimeSinceAttack > .9f) {
            PerformAttack();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            _animator.SetTrigger(PowerHash);
        }
    }

    void PerformAttack()
    {
        _aim.AimAndAttack();

        CurrentAttack = (CurrentAttack % 3) + 1;
        _animator.SetTrigger(AttackHashes[CurrentAttack - 1]);

        TimeSinceAttack = 0;
    }
    public void GetDie()
    {
        _animator.Play("death_idle");
    }
    //triger is attacking at animation character
    void isAttack() => IsAttacking = true;
    void NonAttack() => IsAttacking = false;
}
