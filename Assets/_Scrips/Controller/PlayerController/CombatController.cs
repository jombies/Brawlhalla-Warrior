using System.Collections;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] float attackCooldown = 1.2f;
    [SerializeField] float dashCooldown = 1.5f;
    [SerializeField] float powerCooldown = 6f;
    private float lastAttackTime = -999f;
    private float lastDashTime = -999f;
    private float lastPowerTime = -999f;

    [SerializeField] SkillCooldownUI skillCooldownUI;

    CharacterAnimation anim;
    CharacterController controller;
    Vector3 dashDirection;

    private void Start()
    {
        anim = GetComponentInChildren<CharacterAnimation>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleAttack();
        HandlePower();
        HandleDash();
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time - lastAttackTime >= attackCooldown) {
            lastAttackTime = Time.time;
            anim.DoAttack();
            // UIManager.Instance?.StartCooldownUI("attack", attackCooldown); // Gửi cooldown lên UI
        }
    }

    void HandlePower()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && skillCooldownUI.IsPowerReady() && GameManager.Instance.LoadedData.level >= 2 /*Time.time - lastPowerTime >= powerCooldown*/) {
            lastPowerTime = Time.time;
            skillCooldownUI.StartPowerCooldown();
            anim.DoPowerAttack();
            //UIManager.Instance?.StartCooldownUI("power", powerCooldown);
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && skillCooldownUI.IsDashReady() /*Time.time - lastDashTime >= dashCooldown*/) {
            lastDashTime = Time.time;
            dashDirection = GetCurrentMoveDirection();
            skillCooldownUI.StartDashCooldown();
            StartCoroutine(DoDash());
            //UIManager.Instance?.StartCooldownUI("dash", dashCooldown);
        }
    }

    Vector3 GetCurrentMoveDirection()
    {
        Vector3 camForward = CameraController.Instance.GetCamera().transform.forward;
        Vector3 camRight = CameraController.Instance.GetCamera().transform.right;

        camForward.y = 0;
        camRight.y = 0;

        Vector3 dir = camForward * InputSingleton.instance.Direction.z + camRight * InputSingleton.instance.Direction.x;
        return dir.normalized;
    }

    IEnumerator DoDash()
    {
        float dashDuration = 0.2f;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration) {
            controller.Move(dashDirection * 20 * Time.deltaTime);
            yield return null;
        }

        if (dashDirection.magnitude > 0.1f)
            anim.DoDash();
    }
}
