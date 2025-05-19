using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSenso : MonoBehaviour
{
    [SerializeField] GameObject PartGates;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (PartGates.TryGetComponent<RoomController>(out RoomController room)) room.GateClose();
            if (PartGates.TryGetComponent<BossRoomControler>(out BossRoomControler roomBoss)) roomBoss.GateClose();
            Debug.Log("Player go thought gate");
        }

    }
    private void Reset()
    {
        PartGates = transform.parent.gameObject;
    }
}

