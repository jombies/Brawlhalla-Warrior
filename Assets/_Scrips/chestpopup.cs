using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestpopup : MonoBehaviour
{

    [SerializeField] bool isOpen = false;
    [SerializeField] int Mincoin, Maxcoin;
    [SerializeField] GameObject CoinPrefabs;
    GameObject chestLid;

    [SerializeField] List<DropItem> dropItems = new List<DropItem>();
    [SerializeField] float itemDropHeight = 0.6f;

    [Header("Coin Force Settings")]
    [SerializeField] float minForce = 0.9f;  // Lực văng tối thiểu
    [SerializeField] float maxForce = 1.8f;  // Lực văng tối đa
    [SerializeField] float upwardForce = 5f; // Lực hướng lên

    void Start()
    {
        InitializeChest();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P)) {
            OpenChest();
        }
    }

    private void InitializeChest()
    {
        chestLid = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !isOpen*/) {
            OpenChest();
            AudioManager.Instance.PlaySFX("chestOpen");
        }
    }

    void OpenChest()
    {
        //if (isOpen) return;
        chestLid.transform.DOLocalRotate(new Vector3(80, 0, 0), 1, RotateMode.Fast);
        StartCoroutine(PopUpCoin());
        isOpen = true;
    }

    IEnumerator PopUpCoin()
    {
        if (!isOpen) {
            yield return new WaitForSeconds(.2f);
            int numberOfCoin = Random.Range(Mincoin, Maxcoin);

            for (int i = 0; i < numberOfCoin; i++) {
                SpawnAndLaunchCoin();
                yield return new WaitForSeconds(0.05f); // Độ trễ giữa các đồng xu
            }
            TryDropItem();
        }
    }
    void TryDropItem()
    {
        foreach (var drop in dropItems) {
            float roll = Random.value; // Giá trị từ 0 đến 1
            if (roll <= drop.dropRate && drop.itemPrefab != null) {
                Vector3 spawnPos = transform.position + new Vector3(-2, itemDropHeight, 0);
                ObjectPoolManager.Instance.Spawn(drop.itemPrefab, spawnPos, Quaternion.identity);
                Debug.Log($"🎁 Dropped: {drop.itemPrefab.name} (tỉ lệ {drop.dropRate * 100}%)");
                break; // Nếu chỉ muốn rơi 1 item
            }
        }
    }
    void SpawnAndLaunchCoin()
    {
        GameObject coin = ObjectPoolManager.Instance.Spawn(CoinPrefabs, transform);
        // Đặt vị trí ban đầu
        coin.transform.localPosition = new Vector3(
            Random.Range(-0.5f, 0.5f),
            0.5f,
            Random.Range(-0.5f, 0.5f)
        );

        // Lấy component Rigidbody
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.isKinematic = false;
            // Tạo hướng ngẫu nhiên
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

            // Tính lực ngẫu nhiên
            float randomForce = Random.Range(minForce, maxForce);

            // Thêm lực theo hướng ngẫu nhiên và hướng lên
            Vector3 finalForce = (randomDirection + Vector3.up * upwardForce) * randomForce;

            // Áp dụng lực
            rb.AddForce(finalForce, ForceMode.Impulse);

            // Thêm xoay ngẫu nhiên
            rb.AddTorque(Random.insideUnitSphere * randomForce, ForceMode.Impulse);
        }
        else {
            Debug.LogWarning("Coin prefab không có Rigidbody!");
        }
    }
}
[System.Serializable]
public struct DropItem
{
    public GameObject itemPrefab;
    [Range(0f, 1f)]
    public float dropRate; // 0.2f = 20%
}