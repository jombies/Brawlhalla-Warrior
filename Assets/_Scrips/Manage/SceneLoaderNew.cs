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

    public void loadScene(string sceneName, Action onSceneLoaded = null)
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

        while (loadScene.progress < 0.9f) {
            _progressBar.fillAmount = loadScene.progress;
            _percentText.text = $"Loading... {Mathf.RoundToInt(loadScene.progress * 100)}%";
            yield return null;
        }

        // Scene load xong, cho phép activate
        _progressBar.fillAmount = 1f;
        _percentText.text = $"Loading... 100%";

        loadScene.allowSceneActivation = true;

        // Đợi scene thực sự active
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
