using UnityEditor;
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
        if (TimeSinceAttack < 1.5) TimeSinceAttack += Time.deltaTime;

        if (IsAttacking) return;
        MovingAnimte();
    }

    void MovingAnimte()
    {
        Vector3 direction = InputSingleton.instance.Direction;
        float inputMagnitude = direction.magnitude;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float movementSpeed = 0f;

        if (inputMagnitude > 0.1f) {
            movementSpeed = isRunning ? 1f : 0.5f;
        }

        _animator.SetFloat(VelocityHash, movementSpeed, 0.1f, Time.deltaTime);
    }
    public void DoAttack()
    {
        _aim.AimAndAttack();

        CurrentAttack = (CurrentAttack % 3) + 1;
        _animator.SetTrigger(AttackHashes[CurrentAttack - 1]);

        TimeSinceAttack = 0;
        AudioManager.Instance.PlaySFX("slash" + CurrentAttack);
    }
    public void DoPowerAttack()
    {
        _animator.SetTrigger(PowerHash);
        AudioManager.Instance.PlaySFX("power_attack");
    }

    public void DoDash()
    {
        _animator.SetTrigger("Dash");
    }
    public void GetDie()
    {
        _animator.Play("death_idle");
        AudioManager.Instance.PlaySFX("playerDead");
    }
    //triger is attacking at animation character
    void isAttack() => IsAttacking = true;
    void NonAttack() => IsAttacking = false;
}
