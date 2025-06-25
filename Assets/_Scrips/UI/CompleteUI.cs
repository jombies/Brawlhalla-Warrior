using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : MonoBehaviour
{
    [Header("button Elements")]
    [SerializeField] Button OnHome;
    [SerializeField] Button OnRestart;

    [Header("Label Elements")]
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI GoldText;

    private void Start()
    {
        OnHome.onClick.AddListener(OnHomeClick);
        OnRestart.onClick.AddListener(OnRestartClick);
    }

    private void OnRestartClick()
    {
        SceneLoaderNew.i.loadScene("Testing");
        gameObject.SetActive(false);
    }

    private void OnHomeClick()
    {
        SceneLoaderNew.i.loadScene("Home UI");
        gameObject.SetActive(false);
    }
}
