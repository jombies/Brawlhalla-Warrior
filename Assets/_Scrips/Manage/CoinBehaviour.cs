using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    float speed = 2;
    [SerializeField] bool isMax;
    [SerializeField] Vector3 max;
    [SerializeField] Vector3 min;
    [SerializeField] Vector3 targetP;
    [SerializeField] float value;
    //void Start()
    //{
    //    min = new Vector3(transform.position.x, transform.position.y - value, transform.position.z);
    //    max = new Vector3(transform.position.x, transform.position.y + value, transform.position.z);
    //    targetP = max;
    //}
    void Update()
    {
        TurnAround();
    }
    void TurnAround()
    {
        transform.Rotate(new Vector3(0, 1, 0) * speed);
        //transform.position = Vector3.MoveTowards(transform.position, targetP, Random.Range(0.2f, 0.5f) * Time.deltaTime);
        //if (transform.position == targetP)
        //{
        //    isMax = !isMax;
        //    if (isMax)
        //    {
        //        targetP = min;
        //    }
        //    else targetP = max;
        //}
    }
}
