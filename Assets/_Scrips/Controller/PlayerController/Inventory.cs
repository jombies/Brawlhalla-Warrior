using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public List<Item> Items;
    public int coin;
    readonly int _space = 20;
    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallBack;
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Items = new List<Item>();
    }
    public void CoinCollected(GameObject coina)
    {
        coin++;
        this.PostEvent(EventID.OnCoinCollected, coin);
        if (coina.TryGetComponent<PooledObject>(out PooledObject pooledObject)) {
            pooledObject.ReturnToPool();
        }
        else {
            Destroy(coina);
        }

    }
    public bool Add(Item item)
    {
        if (!item.DefaultItem) {
            if (Items.Count >= _space) {
                Debug.Log("Inventory is full");
                return false;
            }
            Items.Add(item);
            Debug.Log($"[Inventory] Đã thêm item: {item.name}");

            if (OnItemChangedCallBack != null)
                OnItemChangedCallBack.Invoke();
            else
                Debug.LogWarning("[Inventory] OnItemChangedCallBack bị null!");
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
