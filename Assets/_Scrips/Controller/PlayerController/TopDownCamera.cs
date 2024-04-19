using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    Transform _target;
    public float _Height;
    public float _distance;
    public float _angle;
    private Vector3 refVeclocity;

    // Start is called before the first frame update
    void Start()
    {
        _target = PlayerReferences.Instance.Player.transform;
        HandelCamera();
    }

    // Update is called once per frame
    void Update()
    {
        HandelCamera();
    }
    private void HandelCamera()
    {
        if (!_target) return;
        Vector3 worldPos = (Vector3.forward * -_distance) + (Vector3.up * _Height);
        Debug.DrawRay(_target.position, worldPos, Color.red);

        Vector3 rotatedvector = Quaternion.AngleAxis(_angle, Vector3.up) * worldPos;
        Debug.DrawRay(_target.position, rotatedvector, Color.green);

        Vector3 playerPos = _target.position;
        playerPos.y = 0;
        Vector3 finalPos = playerPos + rotatedvector;
        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref refVeclocity, .1f);
        transform.LookAt(playerPos);
    }

}
