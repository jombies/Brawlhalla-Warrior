using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    GameObject player;
    public Transform respawnPoint;
    public float respawnValue = -10;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>().gameObject;

    }
    private void OnEnable()
    {
        player = GetComponent<PlayerController>().gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (respawnPoint == null) {
            respawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        }
        else if (player.transform.position.y < respawnValue) {
            player.transform.position = respawnPoint.position;
        }
    }
}