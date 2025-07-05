using System;
using TMPro;
using Unity.VisualScripting;
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

    PlayerData PlayerData;
    int gold;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerData = PlayerDataManager.Load();
        OnHome.onClick.AddListener(OnHomeClick);
        //OnRestart.onClick.AddListener(OnRestartClick);
        GetResultData();
    }

    private void OnRestartClick()
    {
        SceneLoaderNew.i.LoadScene("Testing");
        gameObject.SetActive(false);
    }


    private void OnHomeClick()
    {
        GameManager.Instance.CleanUpSystemsForHome();
        SceneLoaderNew.i.LoadScene("Home UI");
        Time.timeScale = 1f;
        gameObject.SetActive(false);

    }
    void GetResultData()
    {
        int score = GameSummaryManager.Instance.totalEnemyKilled * 10 + GameSummaryManager.Instance.totalBossKilled * 50;
        gold = (int)Math.Round(GameSummaryManager.Instance.totalEnemyKilled * 5.5f + GameSummaryManager.Instance.totalBossKilled * 10 + Inventory.Instance.coin);
        ScoreText.text = score.ToString();
        GoldText.text = gold.ToString();
        OnSave(score, gold);
    }
    void OnSave(int score, int gold)
    {
        PlayerData.coins += gold;
        PlayerDataManager.Save(PlayerData);
        GameSummaryManager.Instance.ResetSession();
        Debug.Log($"Saved Score: {score}, Gold: {gold}");
    }
}
