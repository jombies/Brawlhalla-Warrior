using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public Camera currentCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateMainCamera();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayUpdateCamera());
    }

    IEnumerator DelayUpdateCamera()
    {
        yield return null;
        UpdateMainCamera();
    }

    public void UpdateMainCamera()
    {
        currentCamera = Camera.main;

        if (currentCamera == null)
            Debug.LogWarning("[CameraController] NO tag 'MainCamera'!");
    }

    public Camera GetCamera()
    {
        if (currentCamera == null) {
            UpdateMainCamera();
        }
        return currentCamera;
    }
}
