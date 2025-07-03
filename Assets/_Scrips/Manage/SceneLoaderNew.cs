using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderNew : MonoBehaviour
{
    public static SceneLoaderNew i;
    Action OnSceneLoadedCallback;

    [SerializeField] GameObject Panel;
    [SerializeField] Image _progressBar;
    [SerializeField] TextMeshProUGUI _percentText;
    [SerializeField] float minLoadTime = 1.5f;

    private string currentSceneName;

    private void Awake()
    {
        if (i == null) {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, Action onSceneLoaded = null)
    {
        Panel.SetActive(true);
        StartCoroutine(LoadSceneAdditive(sceneName, onSceneLoaded));
        OnSceneLoadedCallback = onSceneLoaded;
    }

    IEnumerator LoadSceneAdditive(string sceneName, Action onComplete)
    {
        string sceneToUnload = currentSceneName;

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadScene.allowSceneActivation = false;

        float timer = 0f;

        while (loadScene.progress < 0.9f) {
            timer += Time.deltaTime;
            float fakeProgress = Mathf.Clamp01(timer / minLoadTime);
            float displayProgress = Mathf.Min(loadScene.progress, fakeProgress);

            _progressBar.fillAmount = displayProgress;
            _percentText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100)}%";
            yield return null;
        }

        // Bắt buộc chờ thêm nếu fake progress chưa đủ minLoadTime
        while (timer < minLoadTime) {
            timer += Time.deltaTime;
            float fakeProgress = Mathf.Clamp01(timer / minLoadTime);
            _progressBar.fillAmount = fakeProgress;
            _percentText.text = $"Loading... {Mathf.RoundToInt(fakeProgress * 100)}%";
            yield return null;
        }

        _progressBar.fillAmount = 1f;
        _percentText.text = $"Loading... 100%";

        loadScene.allowSceneActivation = true;

        while (!loadScene.isDone)
            yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        if (loadedScene.IsValid())
            SceneManager.SetActiveScene(loadedScene);

        if (!string.IsNullOrEmpty(sceneToUnload) && sceneToUnload != sceneName) {
            if (SceneManager.GetSceneByName(sceneToUnload).isLoaded)
                SceneManager.UnloadSceneAsync(sceneToUnload);
        }

        currentSceneName = sceneName;
        OnSceneLoadedCallback?.Invoke();
        Panel.SetActive(false);
        resetOnDone();
    }


    public void unloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }

    private void resetOnDone()
    {
        _progressBar.fillAmount = 0f;
        _percentText.text = "";
    }

    private void Reset()
    {
        Panel = transform.Find("loadingPanel").gameObject;
        _progressBar = Panel.transform.Find("fill image").GetComponent<Image>();
        _percentText = Panel.transform.Find("Loading text").GetComponent<TextMeshProUGUI>();
    }
}
