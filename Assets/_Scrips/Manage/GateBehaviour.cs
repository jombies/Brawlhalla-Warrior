using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    public List<GameObject> Gates;
    [SerializeField] float HieghtOfGate;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Delete))
        {
            GateOpen();
        }
    }
    public void GateOpen()
    {
        transform.DOMoveY(-2, 1);
    }
    public void GateClose()
    {
        transform.DOMoveY(HieghtOfGate, 1);
        transform.parent.GetComponent<EnemySpawns>().ExecuteSpawn();
    }

    private void Reset()
    {
        // bossMoving = GameObject.FindGameObjectWithTag("BossEnemy").transform.GetChild(3).GetComponent<BossMoving>();
        foreach (Transform go in transform)
        {
            Gates.Add(go.gameObject);
        }
    }
}
