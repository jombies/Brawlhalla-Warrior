using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0f, 360)]
    public float angle;

    public GameObject Player;

    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask obstructionMask;

    public bool canSeePlayer;
    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerReferences.Instance.Player.gameObject;
        StartCoroutine(FovRoutine());
    }
    IEnumerator FovRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, TargetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < angle / 2)
            {
                float disToTarget = Vector3.Distance(transform.position, dirToTarget);
                if (!Physics.Raycast(transform.position, dirToTarget, disToTarget, obstructionMask)) canSeePlayer = true;
                else canSeePlayer = false;
            }
            else canSeePlayer = false;
        }
        else if (canSeePlayer) canSeePlayer = false;
    }
}