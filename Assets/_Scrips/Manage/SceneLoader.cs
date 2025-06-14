using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader i;
    Action OnSceneLoadedCallback;
    [SerializeField] GameObject Panel;
    [SerializeField] Image _loadingBar;
    [SerializeField] TextMeshProUGUI _percentText;
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
    private void Start()
    {

    }
    public void loadScene(string sceneName, Action onSceneloaded = null)
    {
        OnSceneLoadedCallback = onSceneloaded;
        Panel.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }
    private bool _isPausedAt50Percent = false;
    float _fakeProgress = 0f;
    IEnumerator LoadScene(string sceneName)
    {

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName);
        loadScene.allowSceneActivation = false;

        while (true) {

            float realProgress = loadScene.progress / 0.9f;
            _fakeProgress = Mathf.MoveTowards(_fakeProgress, realProgress, Time.deltaTime * 0.5f);

            float displayProgress = Mathf.Clamp01(_fakeProgress);
            _loadingBar.fillAmount = displayProgress;
            _percentText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100)}%";

            if (displayProgress >= 0.5f && !_isPausedAt50Percent) {
                _isPausedAt50Percent = true;
                yield return new WaitForSeconds(1f);
            }

            if (realProgress >= 1f && _fakeProgress >= 1f) {
                OnSceneLoadedCallback?.Invoke();

                loadScene.allowSceneActivation = true; resetOnDone();
                Panel.SetActive(false);
                yield break;
            }

            yield return null;
        }

    }

    private void resetOnDone()
    {
        _loadingBar.fillAmount = 0f;
    }

    private void Reset()
    {
        Panel = transform.Find("loadingPanel").gameObject;
        _loadingBar = Panel.transform.Find("fill image").GetComponent<Image>();
        _percentText = Panel.transform.Find("Loading text").GetComponent<TextMeshProUGUI>();
    }
}
