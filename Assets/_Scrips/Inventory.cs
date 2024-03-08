using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Inventory
    public static Inventory Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Item> Items = new List<Item>();
    public int coin { get; private set; }
    [SerializeField] TextMeshProUGUI _textCoin;
    readonly int _space = 20;

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;
    public void CoinCollected(GameObject coin)
    {
        this.coin++;
        Destroy(coin);
        _textCoin.text = this.coin.ToString();
    }
    public bool Add(Item item)
    {
        if (!item.DefaultItem)
        {
            if (Items.Count >= _space)
            {
                Debug.Log("Inventory is full");
                return false;
            }
            Items.Add(item);
            if (OnItemChangedCallBack != null)
                OnItemChangedCallBack.Invoke();
        }
        return true;
    }
    public void Remove(Item item)
    {
        Items.Remove(item);
        if (OnItemChangedCallBack != null)
            OnItemChangedCallBack.Invoke();

    }
}
