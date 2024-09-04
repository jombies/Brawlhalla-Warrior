using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    GameObject TheLid;
    bool isOpen = false;
    [SerializeField] int Mincoin, Maxcoin;
    [SerializeField] float x;
    [SerializeField] GameObject CoinPrefabs;
    [SerializeField] GameObject EquipPrefab;
    float numberOfCoin;

    // Start is called before the first frame update
    void Start()
    {
        TheLid = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            OpenChest();
        }
    }
    void OpenChest()
    {
        TheLid.transform.DOLocalRotate(new Vector3(x, 0, 0), 1, RotateMode.Fast);
        StartCoroutine(PopUpCoin());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenChest();
        }
    }

    IEnumerator PopUpCoin()
    {
        if (!isOpen)
        {
            isOpen = true;
            yield return new WaitForSeconds(.2f);
            numberOfCoin = Random.Range(Mincoin, Maxcoin);
            for (int i = 0; i < numberOfCoin; i++)
            {
                GameObject coin = Instantiate(CoinPrefabs, transform);
                coin.transform.localPosition = new Vector3(Random.Range(-2, 2), 0.5f, Random.Range(-2, 2));
            }
            if (EquipPrefab != null)
            {
                GameObject Equip = Instantiate(EquipPrefab, transform);
                Equip.transform.localPosition = new Vector3(3, -1, 2);
            }
            else Debug.Log("KO co Equipment nao!!");
        }

    }
}
