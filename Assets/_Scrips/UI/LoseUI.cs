using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    [Header("button Elements")]
    [SerializeField] Button OnHome;
    //[SerializeField] Button OnRestart;

    [Header("Label Elements")]
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI GoldText;

    private void Start()
    {
        OnHome.onClick.AddListener(OnHomeClick);
        //OnRestart.onClick.AddListener(OnRestartClick);
    }

    private void OnRestartClick()
    {
        SceneLoaderNew.i.loadScene("Testing");
        gameObject.SetActive(false);
    }

    private void OnHomeClick()
    {
        GameManager.Instance.CleanUpSystemsForHome();
        SceneLoaderNew.i.loadScene("Home UI");
        Time.timeScale = 1f;
        gameObject.SetActive(false);

    }
}
