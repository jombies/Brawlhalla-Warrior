using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControler : MonoBehaviour
{
    int radius = 8;

    Transform target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerLocation.Instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        LoadNav();
    }
    private void LoadNav()
    {
        GetComponent<NavMeshAgent>().stoppingDistance = 3;
    }


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= radius)
        {
            agent.SetDestination(target.position);
            if (distance < agent.stoppingDistance)
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
        transform.rotation = Quaternion.Lerp(transform.rotation, faceOff, Time.deltaTime * 10f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
