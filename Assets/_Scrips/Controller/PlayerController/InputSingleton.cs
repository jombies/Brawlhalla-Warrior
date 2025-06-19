using UnityEngine;

public class InputSingleton : MonoBehaviour
{
    private static InputSingleton _mInstance;
    public static InputSingleton instance => _mInstance;
    Vector3 direction;
    public Vector3 Direction { get { return direction; } }


    private void Awake()
    {
        if (_mInstance != null && _mInstance != this) {
            Destroy(gameObject);
            return;
        }
        _mInstance = this;

        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        Getdirection();
    }


    void Getdirection()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }


}
