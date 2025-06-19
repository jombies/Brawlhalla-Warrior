using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool _isPausedAt50Percent = false;
    private float _fakeProgress = 0f;

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
    public void loadScene(string sceneName, Action onSceneloaded = null)
    {
        Panel.SetActive(true);
        StartCoroutine(LoadSceneAdditive(sceneName));
        OnSceneLoadedCallback = onSceneloaded;
    }

    IEnumerator LoadSceneAdditive(string sceneName)
    {
        string sceneToUnload = currentSceneName;

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadScene.allowSceneActivation = false;

        while (true) {
            float realProgress = loadScene.progress / 0.9f;
            _fakeProgress = Mathf.MoveTowards(_fakeProgress, realProgress, Time.deltaTime * 0.5f);

            float displayProgress = Mathf.Clamp01(_fakeProgress);
            _progressBar.fillAmount = displayProgress;
            _percentText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100)}%";

            if (displayProgress >= 0.5f && !_isPausedAt50Percent) {
                _isPausedAt50Percent = true;
                yield return new WaitForSeconds(0);//thay doi nhen giay
            }

            if (realProgress >= 1f && _fakeProgress >= 1f) {
                loadScene.allowSceneActivation = true;
                yield return null;

                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                if (loadedScene.IsValid())
                    SceneManager.SetActiveScene(loadedScene);

                if (!string.IsNullOrEmpty(sceneToUnload) && sceneToUnload != sceneName) {
                    if (SceneManager.GetSceneByName(sceneToUnload).isLoaded) {
                        SceneManager.UnloadSceneAsync(sceneToUnload);
                    }
                }

                currentSceneName = sceneName;

                OnSceneLoadedCallback?.Invoke();
                Panel.SetActive(false);
                resetOnDone();
                yield break;
            }

            yield return null;
        }
    }

    public void unloadScene(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded) {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    private void resetOnDone()
    {
        _progressBar.fillAmount = 0f;
        _isPausedAt50Percent = false;
        _fakeProgress = 0f;
    }

    private void Reset()
    {
        Panel = transform.Find("loadingPanel").gameObject;
        _progressBar = Panel.transform.Find("fill image").GetComponent<Image>();
        _percentText = Panel.transform.Find("Loading text").GetComponent<TextMeshProUGUI>();
    }
}
