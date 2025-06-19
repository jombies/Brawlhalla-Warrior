using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime;
    Vector3 _currVelocity = Vector3.zero;
    public Vector3 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerReferences.Instance.Player.transform;
        //_offset = transform.position - _target.position;
        _offset = transform.position - new Vector3(-1.2f, 0, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 target = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _currVelocity, _smoothTime);

    }
}
