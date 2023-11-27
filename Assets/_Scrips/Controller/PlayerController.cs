
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public CharacterAmintor amintor;

    float speedMax = 5;
    float speedRotation = 540;
    float gra = 8f;
    Vector3 gravity;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        amintor = GetComponentInChildren<CharacterAmintor>();
    }
    private void Update()
    {
        Moving();
    }
    public void Moving()
    {
        Vector3 direction = InputSingleton.Instance.direction;
        float speed = 2;
        if (amintor.IsAttacking) return;
        if (direction.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speedMax;
            }
            controller.Move(direction * speed * Time.deltaTime);
        }
        if (direction != Vector3.zero)
        {
            Quaternion toRotarion = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotarion, speedRotation * Time.deltaTime);
        }
        if (gravity.y < 0) gravity.y = -9.8f;
        gravity.y -= gra * Time.deltaTime;
        controller.Move(gravity * Time.deltaTime);

    }

}
