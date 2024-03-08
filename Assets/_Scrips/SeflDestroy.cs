using UnityEngine;

public class SeflDestroy : MonoBehaviour
{
    [SerializeField] float timeDestroy;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) { Destroy(gameObject); }
    }
}
