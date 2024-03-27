using System.Collections;
using UnityEngine;

public class BossMoving : MonoBehaviour
{
    BossController controller;

    //react
    public bool isfirst = true;
    public bool PlayerOnGr = false;

    // Start is called before the first frame update
    private void Start()
    {
        controller = transform.parent.GetComponent<BossController>();
    }
    void Update()
    {
        if (PlayerOnGr)
        {
            controller.Animator.enabled = true;
            if (!controller.AlreadyFoundPlayer() && !controller.IsAttack)
            {
                GetMoving();
            }
            if (controller.AlreadyFoundPlayer())
            {
                StopWalking();
            }
        }
        else controller.Animator.enabled = false;

    }
    public void GetMoving()
    {
        controller.FaceTarget();
        controller.Agent.SetDestination(controller.Target.transform.position);
        controller.Animator.SetBool("walk", true);
    }
    public void StopWalking()
    {
        controller.Animator.SetBool("walk", false);
        controller.Agent.SetDestination(transform.position);
        if (isfirst)
        {
            controller.Animator.Play("Taunting");
            isfirst = false;
            //controller.EnemyStats.Animator.SetTrigger("taunting");
            //StartCoroutine(FinishAnime());
            //return;
        }
    }


    IEnumerator FinishAnime()
    {
        yield return new WaitForSeconds(3);
        isfirst = false;
    }
}
