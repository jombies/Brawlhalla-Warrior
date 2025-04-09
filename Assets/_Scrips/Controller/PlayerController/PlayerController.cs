using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    PlayerStat PlayerStat;
    CharacterController Controller;
    CharacterAnimation Animater;

    [Header("Movement Settings")]
    [SerializeField] float _speedMax = 5;
    [SerializeField] float _speedRotation = 15;
    [SerializeField] float _gra = 8f;
    float speed = 2.8f;
    Vector3 direction;
    Vector3 _gravity;
    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();
        Controller = GetComponent<CharacterController>();
        Animater = GetComponentInChildren<CharacterAnimation>();
    }
    private void Update()
    {
        direction = InputSingleton.instance.direction;
        Moving();
    }
    void Moving()
    {
        if (Animater.IsAttacking) return;

        RotatePlayer();

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        speed = isRunning ? _speedMax : 2.8f;

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
            _gravity.y = -9.8f;

        _gravity.y -= _gra * Time.deltaTime;
        Controller.Move(_gravity * Time.deltaTime);
    }

    //Damage nhan vao Player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy weapon")) {
            Transform[] ParentTrans = other.gameObject.GetComponentsInParent<Transform>();
            foreach (Transform t in ParentTrans) {
                if (t.TryGetComponent<EnemyStats>(out var dame) /*&& t.GetComponent<EnemyController>().IsAttack*/) {
                    PlayerStat.TakeDamage(dame.Damage.BaseValue);
                    return;
                }
            }

        }
        if (other.CompareTag("Enemy bullet")) {
            if (other.TryGetComponent<BulletInit>(out var bulletInit)) {
                PlayerStat.TakeDamage(bulletInit.Damage);
            }
        }
    }
    //Ktra Player di qua cua
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("GateWay"))
    //    {
    //        other.transform.parent.GetComponent<GateBehaviour>().GateClose(other.gameObject);
    //    }
    //}

    IEnumerator delayMethod(float speed, Vector3 dir)
    {
        yield return new WaitForSeconds(3);
        Controller.Move(speed * Time.deltaTime * dir);
    }
}
