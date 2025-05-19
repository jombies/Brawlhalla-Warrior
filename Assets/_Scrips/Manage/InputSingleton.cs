using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    private static InputSingleton _mInstance;
    public static InputSingleton instance => _mInstance;
    float SHorizon; public float horizon { get => SHorizon; }
    float SVertical; public float vertical { get => SVertical; }
    [SerializeField] GameObject _camera;
    Vector3 Direction;
    public Vector3 direction { get { return Direction; } }

    private Vector3 forward;
    private Vector3 right;

    private void Awake()
    {
        if (_mInstance != null && _mInstance != this) {
            Destroy(gameObject);
            return;
        }
        _mInstance = this;
        if (_camera == null) {
            _camera = Camera.main?.gameObject;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UpdateNormalizei();
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != SHorizon || Input.GetAxisRaw("Vertical") != SVertical) {
            Getdirection();
            SetDirection();
        }
        if (transform.eulerAngles != _camera.transform.eulerAngles) {
            UpdateNormalizei();
            // UpdateAngleDirection();
        }
    }

    void UpdateAngleDirection()
    {
        if (_camera == null) return;
        Vector3 currentEulerAngles = this.gameObject.transform.eulerAngles;
        currentEulerAngles = _camera.transform.eulerAngles;
        this.gameObject.transform.eulerAngles = currentEulerAngles;
    }
    void UpdateNormalizei()
    {
        forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        right = transform.right;
        right.y = 0;
        right.Normalize();
    }

    void Getdirection()
    {
        SHorizon = Input.GetAxisRaw("Horizontal");
        SVertical = Input.GetAxisRaw("Vertical");
    }

    void SetDirection()
    {
        Vector3 verticalR = vertical * forward;
        Vector3 horizontalR = horizon * right;
        Direction = (verticalR + horizontalR).normalized;
    }
}
