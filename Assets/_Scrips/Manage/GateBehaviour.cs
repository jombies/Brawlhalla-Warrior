using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    public List<GameObject> gameObjects;
    [SerializeField] float move;
    [SerializeField] GameObject GameObject;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Delete))
        {
            GateOpen(GameObject);
        }
    }
    void ActionGate(GameObject ParObj)
    {
        ParObj.transform.DOMoveY(move, 1);
    }
    public void GateClose(GameObject obj)
    {
        ActionGate(obj.transform.parent.gameObject);
        GameObject = obj;
    }
    public void GateOpen(GameObject go)
    {
        go.transform.parent.DOMoveY(-2, 1);
    }
    private void Reset()
    {
        // bossMoving = GameObject.FindGameObjectWithTag("BossEnemy").transform.GetChild(3).GetComponent<BossMoving>();
        foreach (Transform go in transform)
        {
            gameObjects.Add(go.gameObject);
        }
    }
}
