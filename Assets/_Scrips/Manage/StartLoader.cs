using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoader : MonoBehaviour
{
    [SerializeField] Image _loadingBar;
    [SerializeField] GameObject Home_UI; // The initial scene to load, e.g., "Home UI"
    [SerializeField] float _loadDelay;
    [SerializeField] float _delayPercentage = 0.75f;
    [SerializeField] TextMeshProUGUI _textLoading;

    private void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(LoadLevel("Home UI"));
    }

    IEnumerator LoadLevel(string targetLevel)
    {
        float timeSinceLoad = 0f;
        float sceneLoadPercentage = 1f - _delayPercentage;
        bool pausedAt90Percent = false;
        while (timeSinceLoad < _loadDelay) {
            float progress = Mathf.Clamp01(timeSinceLoad / _loadDelay)/* * _delayPercentage*/;
            if (progress >= 0.9f * _delayPercentage && !pausedAt90Percent) {
                pausedAt90Percent = true;
                yield return new WaitForSeconds(1f);
            }
            _loadingBar.fillAmount = progress;
            if (_textLoading != null)
                _textLoading.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
            timeSinceLoad += Time.deltaTime;
            yield return null;
        }
        //Start doing load
        AsyncOperation loadSceen = SceneManager.LoadSceneAsync(targetLevel);
        while (!loadSceen.isDone) {
            _loadingBar.fillAmount = _delayPercentage + Mathf.Clamp01(loadSceen.progress / .9f) * sceneLoadPercentage;
            yield return null;
        }
        yield return null;
        gameObject.SetActive(false);
        Home_UI.SetActive(true);
    }
}
