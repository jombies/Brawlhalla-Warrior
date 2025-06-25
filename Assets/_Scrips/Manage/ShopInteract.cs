using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopInteract : MonoBehaviour
{
    [SerializeField] GameObject shopItem;
    [SerializeField] LayerMask IsPlayer;
    [SerializeField] float Range = 2f;
    [SerializeField] Canvas shopCanvas;
    private bool playerInRange = false;
    [SerializeField] int price;
    [SerializeField] int xOffset;
    // Start is called before the first frame update
    void Start()
    {
        shopCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, Range, IsPlayer);

        if (playerInRange) {
            OpenShop();
        }
        else {
            shopCanvas.gameObject.SetActive(false);
        }
    }

    private void OpenShop()
    {
        shopCanvas.gameObject.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E)) {
            if (Inventory.Instance.coin >= price) {
                Inventory.Instance.coin -= price;
                Vector3 spawnPos = transform.position + new Vector3(xOffset, 0, 0);
                Instantiate(shopItem, spawnPos, Quaternion.identity);
                this.PostEvent(EventID.OnCoinCollected, Inventory.Instance.coin);
                gameObject.SetActive(false);
            }
            else {
                Debug.Log("Not enough coins!");
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
