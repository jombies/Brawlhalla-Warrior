using UnityEngine;

public class CharacterAmintor : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        movingAnimte();
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

        if (Input.GetKey(KeyCode.F)) animator.Play("attacking1");
    }
}
