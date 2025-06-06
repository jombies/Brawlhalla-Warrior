// Optimized Object Pooling System - Fixed Orientation Issues
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    private Dictionary<int, ObjectPool> pools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PreloadPool(GameObject prefab, int amount)
    {
        int id = prefab.GetInstanceID();
        if (!pools.ContainsKey(id))
            pools[id] = new ObjectPool(prefab);
        pools[id].Preload(amount);
    }

    public GameObject InstantiateFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int id = prefab.GetInstanceID();
        if (!pools.ContainsKey(id)) {
            pools[id] = new ObjectPool(prefab);
            pools[id].Preload(1); // ✅ Auto preload ít nhất 1 object
            Debug.LogWarning($"[ObjectPool] Auto-created pool for {prefab.name}. Consider preloading.");
        }

        return pools[id].Get(position, rotation);
    }

    void Return(GameObject obj, int prefabId)
    {
        if (pools.ContainsKey(prefabId))
            pools[prefabId].Return(obj);
        else
            Destroy(obj);
    }
    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        var pooled = obj.GetComponent<PooledObject>();
        if (pooled != null) {
            Return(obj, pooled.PrefabId);
        }
        else {
            Destroy(obj); // If not pooled, destroy the object
        }
    }
}

public class ObjectPool
{
    private readonly GameObject prefab;
    private readonly int prefabId;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();
    private readonly Transform root;

    // Store original prefab rotation for reset
    private readonly Quaternion originalRotation;
    private readonly Vector3 originalScale;

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
        prefabId = prefab.GetInstanceID();

        // Store original transform values
        originalRotation = prefab.transform.rotation;
        originalScale = prefab.transform.localScale;

        GameObject rootObj = new GameObject(prefab.name + "_Pool");
        root = rootObj.transform;
        root.SetParent(ObjectPoolManager.Instance.transform);
    }

    public void Preload(int count)
    {
        for (int i = 0; i < count; i++) {
            var obj = Create();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = (pool.Count > 0) ? pool.Dequeue() : Create();

        // Reset parent to null first to avoid transform issues
        // obj.transform.SetParent(null);

        // Reset transform to original state
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = originalScale;
        obj.SetActive(true);

        var pooled = obj.GetComponent<PooledObject>();
        pooled?.OnGet();

        //obj.SetActive(true);

        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);

        var pooled = obj.GetComponent<PooledObject>();
        pooled?.OnReturn();

        // Reset transform before returning to pool
        obj.transform.rotation = originalRotation;
        obj.transform.localScale = originalScale;
        obj.transform.SetParent(root);
        obj.transform.localPosition = Vector3.zero;

        pool.Enqueue(obj);
    }

    private GameObject Create()
    {
        var obj = GameObject.Instantiate(prefab);
        var pooled = obj.AddComponent<PooledObject>();
        pooled.PrefabId = prefabId;

        obj.AddComponent<PooledObject>().PrefabId = prefabId;
        obj.transform.SetParent(root);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = originalScale;
        // obj.SetActive(false);
        return obj;
    }
}

public class PooledObject : MonoBehaviour
{
    public int PrefabId;
    public float AutoReturnTime = 0f;
    private float timer;

    public void OnGet()
    {
        timer = AutoReturnTime;
        var poolables = GetComponents<IPoolableObject>();
        foreach (var poolable in poolables) {
            poolable.OnGet();
        }
    }

    public void OnReturn()
    {
        var poolables = GetComponents<IPoolableObject>();
        foreach (var poolable in poolables) {
            poolable.OnReturn();
        }
    }

    private void Update()
    {
        if (AutoReturnTime <= 0f) return;
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            ObjectPoolManager.Instance.ReturnToPool(gameObject);
        }
    }
    public void ReturnToPool(GameObject obj)
    {
        ObjectPoolManager.Instance.ReturnToPool(obj);
    }

}

public interface IPoolableObject
{
    void OnGet();
    void OnReturn();
}