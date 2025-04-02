using DG.Tweening;
using System.Collections;
using UnityEngine;

public class chestpopup : MonoBehaviour
{
    GameObject chestLid;
    bool isOpen = false;
    [SerializeField] int Mincoin, Maxcoin;
    [SerializeField] float x;
    [SerializeField] GameObject CoinPrefabs;
    float numberOfCoin;

    [Header("Coin Force Settings")]
    [SerializeField] float minForce = 5f;  // Lực văng tối thiểu
    [SerializeField] float maxForce = 8f;  // Lực văng tối đa
    [SerializeField] float upwardForce = 5f; // Lực hướng lên

    void Start()
    {
        InitializeChest();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            OpenChest();
        }
    }

    private void InitializeChest()
    {
        chestLid = transform.GetChild(0).gameObject;
        if (chestLid == null)
        {
            Debug.LogError("Không tìm thấy nắp rương!");
        }
    }

    void OpenChest()
    {
        if (isOpen) return;
        chestLid.transform.DOLocalRotate(new Vector3(x, 0, 0), 1, RotateMode.Fast);
        StartCoroutine(PopUpCoin());
        isOpen = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            OpenChest();
        }
    }

    IEnumerator PopUpCoin()
    {
        if (!isOpen)
        {
            yield return new WaitForSeconds(.2f);
            numberOfCoin = Random.Range(Mincoin, Maxcoin);

            for (int i = 0; i < numberOfCoin; i++)
            {
                SpawnAndLaunchCoin();
                yield return new WaitForSeconds(0.05f); // Độ trễ giữa các đồng xu
            }
        }
    }

    void SpawnAndLaunchCoin()
    {
        // Tạo coin
        GameObject coin = Instantiate(CoinPrefabs, transform);

        // Đặt vị trí ban đầu
        coin.transform.localPosition = new Vector3(
            Random.Range(-2f, 2f),
            0.5f,
            Random.Range(-2f, 2f)
        );

        // Lấy component Rigidbody
        Rigidbody rb = coin.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Tắt Kinematic nếu có
            rb.isKinematic = false;

            // Tạo hướng ngẫu nhiên
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)
            ).normalized;

            // Tính lực ngẫu nhiên
            float randomForce = Random.Range(minForce, maxForce);

            // Thêm lực theo hướng ngẫu nhiên và hướng lên
            Vector3 finalForce = (randomDirection + Vector3.up * upwardForce) * randomForce;

            // Áp dụng lực
            rb.AddForce(finalForce, ForceMode.Impulse);

            // Thêm xoay ngẫu nhiên
            rb.AddTorque(Random.insideUnitSphere * randomForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Coin prefab không có Rigidbody!");
        }
    }
}
