
using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    [SerializeField] private static InputSingleton mInstance;
    public static InputSingleton Instance { get => mInstance; }

    protected float sHorizon; public float Horizon { get => sHorizon; }
    protected float sVertical; public float Vertical { get => sVertical; }

    protected Vector3 _direction;
    public Vector3 direction { get { return _direction; } }
    private void Awake()
    {
        mInstance = this;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        Getdirection();
        SetDirection();
    }
    void Getdirection()
    {
        sHorizon = Input.GetAxisRaw("Horizontal");
        sVertical = Input.GetAxisRaw("Vertical");
    }
    void SetDirection()
    {
        Vector3 forward = gameObject.transform.forward; forward.y = 0;
        Vector3 right = gameObject.transform.right; right.y = 0;
        forward = forward.normalized;
        right = right.normalized;
        Vector3 verticalR = Vertical * forward;
        Vector3 horizontalR = Horizon * right;
        _direction = verticalR + horizontalR;
    }
}
