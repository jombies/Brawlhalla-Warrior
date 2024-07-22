using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSenso : MonoBehaviour
{
    [SerializeField] GameObject ParGates;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ParGates.GetComponent<GateBehaviour>().GateClose();
            Debug.Log("Player go thought gate");
        }
    }
    private void Reset()
    {
        ParGates = transform.parent.gameObject;
    }
}
