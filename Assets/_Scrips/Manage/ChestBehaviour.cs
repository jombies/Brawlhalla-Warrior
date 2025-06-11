using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{
    GameObject chestLid;
    [SerializeField] bool isOpen = false;
    [SerializeField] int Mincoin, Maxcoin;
    [SerializeField] float x;
    [SerializeField] GameObject CoinPrefabs;
    float numberOfCoin;
    [SerializeField] float force;

    // Start is called before the first frame update
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
        // Lấy reference đến nắp rương
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
                GameObject coin = Instantiate(CoinPrefabs, transform);
                coin.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
                //Vector3 randonDir = Random.insideUnitSphere * 100;
                //randonDir.y = Mathf.Abs(randonDir.y);
                //Rigidbody rb = coin.GetComponent<Rigidbody>();
                //if (rb != null)
                //{
                //    rb.AddForce(randonDir.normalized * force);
                //}
            }
        }

    }
}
