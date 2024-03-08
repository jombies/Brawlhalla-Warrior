using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerStat PlayerStat;
    private CharacterController Controller;
    private CharacterAnimation Animater;

    readonly float _speedMax = 5;
    readonly float _speedRotation = 540;
    readonly float _gra = 8f;
    Vector3 _gravity;
    private void Awake()
    {
        PlayerStat = GetComponent<PlayerStat>();
        Controller = GetComponent<CharacterController>();
        Animater = GetComponentInChildren<CharacterAnimation>();
    }
    private void Update()
    {
        Moving();
    }
    void Moving()
    {
        Vector3 direction = InputSingleton.instance.direction;
        float speed = 3;
        if (Animater.IsAttacking) return;
        if (direction.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = _speedMax;
            }
            Controller.Move(direction * speed * Time.deltaTime);
        }
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _speedRotation * Time.deltaTime);
        }
        if (_gravity.y < 0) _gravity.y = -9.8f;
        _gravity.y -= _gra * Time.deltaTime;
        Controller.Move(_gravity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var dame = other.transform.root.GetComponent<EnemyStats>();//other.GetComponentInParent<EnemyStats>();
            if (dame != null)
            {
                PlayerStat.TakeDamage(dame.Damage.BaseValue);
            }
        }
        if (other.CompareTag("Bullet-Enemy"))
        {
            if (other.TryGetComponent<BulletInit>(out BulletInit bulletInit))
            {
                PlayerStat.TakeDamage(bulletInit.Damage);
            }
        }
    }
}
