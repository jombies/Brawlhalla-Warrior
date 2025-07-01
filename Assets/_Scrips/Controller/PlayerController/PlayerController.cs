using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    static PlayerController Instance;
    PlayerStat PlayerStat;
    CharacterController Controller;
    CharacterAnimation Animater;

    [Header("Movement Settings")]
    [SerializeField] float _walkSpeed = 2.8f;
    [SerializeField] float _speedMax = 5;
    [SerializeField] float _speedRotation = 15;
    float speed = 2.8f;
    Vector3 direction;
    Vector3 _gravity;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerStat = GetComponent<PlayerStat>();
        Controller = GetComponent<CharacterController>();
        Animater = GetComponentInChildren<CharacterAnimation>();
    }
    private void Update()
    {
        UpdateAngleDirection();
        Moving();
    }

    void UpdateAngleDirection()
    {
        Camera _camera = Camera.current ?? CameraController.Instance.GetCamera();
        Vector3 camForward = _camera.transform.forward;
        Vector3 camRight = _camera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Gộp hướng lại theo input
        direction = camForward * InputSingleton.instance.Direction.z + camRight * InputSingleton.instance.Direction.x;
        if (direction.magnitude > 0.1f) direction.Normalize();
        else direction = Vector3.zero;
    }
    void Moving()
    {
        if (Animater.IsAttacking) return;

        RotatePlayer();

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        speed = isRunning ? _speedMax : _walkSpeed;

        if (direction.magnitude > 0.1f) {
            Controller.Move(direction * Time.deltaTime * speed);
        }
        ApplyGravity();
    }

    void RotatePlayer()
    {
        if (direction != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _speedRotation * Time.deltaTime);
        }
    }
    void ApplyGravity()
    {
        if (Controller.isGrounded && _gravity.y < 0)
            _gravity.y = -2f;

        _gravity.y += Physics.gravity.y * Time.deltaTime;
        Controller.Move(_gravity * Time.deltaTime);
    }

    //Damage nhan vao Player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy weapon")) {
            Transform[] ParentTrans = other.gameObject.GetComponentsInParent<Transform>();
            foreach (Transform t in ParentTrans) {
                if (t.TryGetComponent<EnemyStats>(out var dame) && t.GetComponent<EnemyController>().IsAttack) {
                    PlayerStat.TakeDamage(dame.Damage.Value);
                    return;
                }
            }
        }
    }
}
