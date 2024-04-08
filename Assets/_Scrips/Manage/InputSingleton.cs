using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    [SerializeField] private static InputSingleton _mInstance;
    public static InputSingleton instance { get => _mInstance; }
    protected float SHorizon; public float horizon { get => SHorizon; }
    protected float SVertical; public float vertical { get => SVertical; }

    protected Vector3 Direction;
    public Vector3 direction { get { return Direction; } }
    private void Awake()
    {
        _mInstance = this;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        Getdirection();
        SetDirection();
    }
    void Getdirection()
    {
        SHorizon = Input.GetAxisRaw("Horizontal");
        SVertical = Input.GetAxisRaw("Vertical");
    }
    void SetDirection()
    {
        Vector3 forward = gameObject.transform.forward; forward.y = 0;
        Vector3 right = gameObject.transform.right; right.y = 0;
        forward = forward.normalized;
        right = right.normalized;
        Vector3 verticalR = vertical * forward;
        Vector3 horizontalR = horizon * right;
        Direction = (verticalR + horizontalR).normalized;
    }
}
