using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControler : MonoBehaviour
{
    int radius = 5;

    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerLocation.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < radius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                //doing something
                faceTarget();
            }
        }
    }

    void faceTarget()
    {
        Vector3 Fdir = (target.position - transform.position).normalized;
        Quaternion faceOff = Quaternion.LookRotation(new Vector3(Fdir.x, 0, Fdir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, Time.deltaTime * 5f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
