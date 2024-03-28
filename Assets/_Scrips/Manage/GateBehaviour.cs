using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    [SerializeField] BossMoving bossMoving;
    public List<GameObject> gameObjects;

    void ActionGate()
    {
        foreach (var go in gameObjects)
        {
            go.TryGetComponent<BoxCollider>(out BoxCollider box);
            {
                box.isTrigger = false;
            }
            go.SetActive(false);
        }

    }
    public void GateClose(GameObject go)
    {
        go.GetComponent<BoxCollider>().isTrigger = false;
        go.transform.DOMoveY(-3, 1);
        bossMoving.PlayerOnGr = true;
    }
    public void GateOpen(GameObject go)
    {
        go.GetComponent<BoxCollider>().enabled = false;
        go.transform.DOMoveY(3, 1);

    }
    private void Reset()
    {
        bossMoving = GameObject.FindGameObjectWithTag("BossEnemy").transform.GetChild(3).GetComponent<BossMoving>();
        foreach (Transform go in transform)
        {
            gameObjects.Add(go.gameObject);
        }
    }

}
