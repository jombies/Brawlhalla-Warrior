using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Inventory
    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    List<Item> items = new List<Item>();
    public int Coin { get; private set; }
    [SerializeField] TextMeshProUGUI textCoin;
    int space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;
    public void coinCollected(GameObject coin)
    {
        Coin++;
        Destroy(coin);
        textCoin.text = Coin.ToString();
    }
    public bool Add(Item item)
    {
        if (!item.DefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Invetory is full");
                return false;
            }
            items.Add(item);
            if (onItemChangedCallBack != null)
                onItemChangedCallBack.Invoke();
        }
        return true;
    }
    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallBack != null)
            onItemChangedCallBack.Invoke();
    }
}
