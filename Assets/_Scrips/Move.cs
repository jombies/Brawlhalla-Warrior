
using UnityEngine;

public class Move : MonoBehaviour
{
    public CharacterController controller;
    float speedMax = 5;
    float speedRotation = 540;
    float gra = 8f;
    Vector3 gravity;
    private void Start()
    {
        Application.targetFrameRate = 51;
    }
    private void Update()
    {
        Moving();
    }
    public void Moving()
    {
        Vector3 direction = InputSingleton.Instance.direction;
        float speed = 2;

        if (direction.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = speedMax;
            }
            Debug.Log("s = " + speed);
            controller.Move(direction * speed * Time.deltaTime);
        }
        if (direction != Vector3.zero)
        {
            Quaternion toRotarion = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotarion, speedRotation * Time.deltaTime);
        }
        gravity.y -= gra * Time.deltaTime;
        controller.Move(gravity * Time.deltaTime);



    }
}
