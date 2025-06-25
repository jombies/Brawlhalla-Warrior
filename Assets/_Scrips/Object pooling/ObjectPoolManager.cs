using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public interface IPoolable
{
    void OnSpawnFromPool();
    void OnReturnToPool();
}

public class PooledObject : MonoBehaviour
{
    [Header("Auto Return Settings")]
    [SerializeField] private bool useAutoReturn = false;
    [SerializeField] private float autoReturnTime = 5f;

    private ObjectPool pool;
    private Coroutine autoReturnCoroutine;

    /// <summary>
    /// Thiết lập pool cho object này
    /// </summary>
    public void SetPool(ObjectPool pool)
    {
        this.pool = pool;
    }

    /// <summary>
    /// Bắt đầu auto return timer
    /// </summary>
    public void StartAutoReturn()
    {
        if (useAutoReturn && autoReturnTime > 0 && gameObject.activeInHierarchy) {
            StopAutoReturn(); // Dừng timer cũ nếu có
            autoReturnCoroutine = StartCoroutine(AutoReturnCoroutine());
        }
    }

    /// <summary>
    /// Bắt đầu auto return với time tùy chỉnh
    /// </summary>
    public void StartAutoReturn(float customTime)
    {
        if (customTime > 0 && gameObject.activeInHierarchy) {
            StopAutoReturn();
            autoReturnCoroutine = StartCoroutine(AutoReturnCoroutine(customTime));
        }
    }

    /// <summary>
    /// Dừng auto return timer
    /// </summary>
    public void StopAutoReturn()
    {
        if (autoReturnCoroutine != null) {
            StopCoroutine(autoReturnCoroutine);
            autoReturnCoroutine = null;
        }
    }

    /// <summary>
    /// Trả object về pool
    /// </summary>
    public void ReturnToPool()
    {
        if (pool != null) {
            pool.ReturnObject(gameObject);
        }
    }

    /// <summary>
    /// Thiết lập auto return time runtime và bắt đầu timer ngay
    /// </summary>
    public void SetAutoReturnTime(float time)
    {
        autoReturnTime = time;
        useAutoReturn = time > 0;

        // Bắt đầu timer ngay nếu object đang active
        if (useAutoReturn && gameObject.activeInHierarchy) {
            StartAutoReturn();
        }
    }

    /// <summary>
    /// Thiết lập auto return time mà không bắt đầu timer
    /// </summary>
    public void SetAutoReturnTimeOnly(float time)
    {
        autoReturnTime = time;
        useAutoReturn = time > 0;
    }

    private IEnumerator AutoReturnCoroutine()
    {
        yield return new WaitForSeconds(autoReturnTime);
        ReturnToPool();
    }

    private IEnumerator AutoReturnCoroutine(float customTime)
    {
        yield return new WaitForSeconds(customTime);
        ReturnToPool();
    }

    private void OnDestroy()
    {
        StopAutoReturn();
    }
}

public class ObjectPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 5;
    [SerializeField] private int maxSize = 50;
    [SerializeField] private bool autoExpand = true;

    [Header("Transform Settings")]
    [SerializeField] private Transform poolParent;
    [SerializeField] private bool resetTransformOnReturn = true;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();
    private HashSet<GameObject> activeObjects = new HashSet<GameObject>();
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        if (prefab == null) {
            Debug.LogWarning("Prefab is null! Please assign a prefab to the pool.");
            return;
        }

        originalPosition = prefab.transform.position;
        originalRotation = prefab.transform.rotation;
        originalScale = prefab.transform.localScale;

        if (poolParent == null) {
            poolParent = transform;
        }

        for (int i = 0; i < initialSize; i++) {
            CreateNewObject();
        }
    }

    public void SetupPool(GameObject prefab, int initialSize, int maxSize)
    {
        this.prefab = prefab;
        this.initialSize = initialSize;
        this.maxSize = maxSize;
        InitializePool();
    }

    private GameObject CreateNewObject()
    {
        GameObject newObj = Instantiate(prefab);
        newObj.transform.SetParent(poolParent, false);

        var pooledComponent = newObj.GetComponent<PooledObject>() ?? newObj.AddComponent<PooledObject>();
        pooledComponent.SetPool(this);

        ResetTransform(newObj);
        newObj.SetActive(false);
        availableObjects.Enqueue(newObj);

        return newObj;
    }

    public GameObject GetObject()
    {
        return GetObject(Vector3.zero, Quaternion.identity);
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetAvailableObject();

        if (obj == null) {
            Debug.LogWarning("ObjectPool is full and auto expansion is disabled or max size reached.");
            return null;
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.transform.localScale = originalScale;
        activeObjects.Add(obj);

        var poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawnFromPool();

        obj.SetActive(true);

        var pooled = obj.GetComponent<PooledObject>();
        pooled?.StartAutoReturn();

        return obj;
    }

    private GameObject GetAvailableObject()
    {
        if (availableObjects.Count > 0) {
            return availableObjects.Dequeue();
        }

        if (autoExpand && GetTotalCount() < maxSize) {
            return CreateNewObject();
        }

        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        if (obj == null || !activeObjects.Contains(obj)) {
            return;
        }

        var pooled = obj.GetComponent<PooledObject>();
        pooled?.StopAutoReturn();

        var poolable = obj.GetComponent<IPoolable>();
        poolable?.OnReturnToPool();

        if (resetTransformOnReturn) {
            ResetTransform(obj);
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolParent, false);

        activeObjects.Remove(obj);
        availableObjects.Enqueue(obj);
    }

    private void ResetTransform(GameObject obj)
    {
        obj.transform.position = originalPosition;
        obj.transform.rotation = originalRotation;
        obj.transform.localScale = originalScale;
    }

    public void ReturnAllObjects()
    {
        foreach (var obj in new List<GameObject>(activeObjects)) {
            ReturnObject(obj);
        }
    }

    public int GetActiveCount() => activeObjects.Count;
    public int GetAvailableCount() => availableObjects.Count;
    public int GetTotalCount() => activeObjects.Count + availableObjects.Count;

    public void PreloadObjects(int amount)
    {
        int currentTotal = GetTotalCount();
        int targetTotal = currentTotal + amount;

        if (targetTotal > maxSize) {
            amount = maxSize - currentTotal;
            if (amount <= 0) {
                Debug.LogWarning($"Pool {prefab.name} is already at max capacity ({maxSize})");
                return;
            }
        }

        for (int i = 0; i < amount; i++) {
            CreateNewObject();
        }

        Debug.Log($"Preloaded {amount} objects to pool {prefab.name}. Total: {GetTotalCount()}");
    }
}


/// <summary>
/// Object Pool Manager - Quản lý nhiều pools
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null) {
                instance = FindObjectOfType<ObjectPoolManager>();
                if (instance == null) {
                    GameObject go = new GameObject("ObjectPoolManager");
                    instance = go.AddComponent<ObjectPoolManager>();
                }
            }
            return instance;
        }
    }

    private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Tạo pool mới
    /// </summary>
    public ObjectPool CreatePool(GameObject prefab, int initialSize = 10, int maxSize = 50)
    {
        if (prefab == null) {
            Debug.LogError("Prefab is null!");
            return null;
        }

        if (pools.ContainsKey(prefab)) {
            Debug.LogWarning($"Pool for prefab {prefab.name} already exists!");
            return pools[prefab];
        }

        // Tạo pool dưới poolsContainer
        GameObject poolObject = new GameObject($"Pool_{prefab.name}");
        poolObject.transform.SetParent(transform);

        ObjectPool pool = poolObject.AddComponent<ObjectPool>();
        pool.SetupPool(prefab, initialSize, maxSize);

        pools[prefab] = pool;
        return pool;
    }

    /// <summary>
    /// Preload pool - Tạo sẵn objects để sử dụng ngay lập tức
    /// </summary>
    public void PreloadPool(GameObject prefab, int amount)
    {
        if (prefab == null) {
            Debug.LogError("Prefab is null!");
            return;
        }

        ObjectPool pool = GetPool(prefab);
        if (pool == null) {
            // Tạo pool mới với amount làm initial size
            pool = CreatePool(prefab, amount, Mathf.Max(amount * 2, 50));
        }
        else {
            // Pool đã tồn tại, preload thêm objects
            pool.PreloadObjects(amount);
        }

        Debug.Log($"Preloaded {amount} objects for {prefab.name}. Pool now has {pool.GetTotalCount()} objects.");
    }

    /// <summary>
    /// Lấy pool theo prefab
    /// </summary>
    public ObjectPool GetPool(GameObject prefab)
    {
        if (prefab == null) return null;

        pools.TryGetValue(prefab, out ObjectPool pool);
        return pool;
    }

    /// <summary>
    /// Spawn object từ pool (tự động tạo pool nếu chưa có)
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position = default, Quaternion rotation = default)
    {
        if (prefab == null) {
            Debug.LogError("Prefab is null!");
            return null;
        }

        ObjectPool pool = GetPool(prefab);
        if (pool == null) {
            // Tự động tạo pool nếu chưa có
            pool = CreatePool(prefab);
        }

        if (pool != null) {
            return pool.GetObject(position, rotation);
        }

        Debug.LogError($"Failed to create pool for prefab {prefab.name}!");
        return null;
    }

    /// <summary>
    /// Spawn object từ pool với Transform
    /// </summary>
    public GameObject Spawn(GameObject prefab, Transform spawnTransform)
    {
        return Spawn(prefab, spawnTransform.position, spawnTransform.rotation);
    }

    /// <summary>
    /// Despawn object về pool
    /// </summary>
    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        PooledObject pooledComponent = obj.GetComponent<PooledObject>();
        pooledComponent?.ReturnToPool();
    }

    /// <summary>
    /// Kiểm tra pool có tồn tại không
    /// </summary>
    public bool HasPool(GameObject prefab)
    {
        return prefab != null && pools.ContainsKey(prefab);
    }

    /// <summary>
    /// Xóa pool
    /// </summary>
    public void RemovePool(GameObject prefab)
    {
        if (prefab != null && pools.ContainsKey(prefab)) {
            ObjectPool pool = pools[prefab];
            pool.ReturnAllObjects();
            pools.Remove(prefab);

            if (pool != null) {
                Destroy(pool.gameObject);
            }
        }
    }

    /// <summary>
    /// Lấy thông tin pool
    /// </summary>
    public string GetPoolInfo(GameObject prefab)
    {
        ObjectPool pool = GetPool(prefab);
        if (pool != null) {
            return $"Pool {prefab.name}: Active={pool.GetActiveCount()}, Available={pool.GetAvailableCount()}, Total={pool.GetTotalCount()}";
        }
        return $"Pool for {prefab.name} not found!";
    }
}