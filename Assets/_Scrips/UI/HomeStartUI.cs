using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeStartUI : MonoBehaviour
{
    [SerializeField] LevelUpgradeData upgradeStats;
    [Header("Buttons")]
    [SerializeField] Button playBtn;
    [SerializeField] Button backBtn;
    [SerializeField] Button upgradeBtn;

    [Header("information frame")]
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider armorSlider;
    [SerializeField] Slider attackSlider;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI armorText;
    [SerializeField] TextMeshProUGUI UpgradeCostText;
    [SerializeField] GameObject UnlockSkill;

    void Start()
    {
        AudioManager.Instance.PlayMusic("musicRoom");
        this.RegisterListener(EventID.UpdateFrameUIData, UpdateFrameUI);
        this.RegisterListener(EventID.UpgradeCostUpdate, UpgradeCost);
        PlayerData data = PlayerDataManager.Load();
        playBtn.onClick.AddListener(OnClickPlay);
        backBtn.onClick.AddListener(OnClickBack);
        upgradeBtn.onClick.AddListener(OnClickUpgrade);
        InitializeInformationFrame(data);
    }
    private void OnDestroy()
    {
        this.RemoveListener(EventID.UpdateFrameUIData, UpdateFrameUI);
        this.RemoveListener(EventID.UpgradeCostUpdate, UpgradeCost);
    }

    private void UpdateFrameUI(object obj)
    {
        InitializeInformationFrame(PlayerDataManager.Load());
    }
    private void UpgradeCost(object obj)
    {
        UpgradeCostText.text = $"{obj} ";
    }
    void InitializeInformationFrame(PlayerData data)
    {
        if (data.level >= 2) UnlockSkill.SetActive(false);
        else UnlockSkill.SetActive(true);
        coinText.text = $"{data.coins}";
        healthSlider.value = data.maxHP;
        armorSlider.value = data.armor;
        attackSlider.value = data.attack;
        healthText.text = $"{data.maxHP}";
        armorText.text = $"{data.armor}";

        if (data.level >= 6) {
            UpgradeCostText.text = "Max Level";
            upgradeBtn.interactable = false;
            return;
        }
        else
            UpgradeCostText.text = $"{upgradeStats.GetStatsForLevel(data.level + 1).cost}";
    }

    private void OnClickUpgrade()
    {
        this.PostEvent(EventID.GetUpgradeData);
    }
    private void OnClickPlay()
    {
        AudioManager.Instance.PlaySFX("btnStart");
        GameManager.Instance.PrepareDataBeforePlay();
        //  SceneManager.LoadScene("GamePlayUI", LoadSceneMode.Additive);
        SceneLoaderNew.i.LoadScene("Testing", () =>
        {
            GameObject player = PlayerReferences.Instance.Player;
            Transform spawn = GameObject.Find("PlayerSpawnPoint")?.transform;
            if (spawn != null) player.transform.position = spawn.position;
            CameraController.Instance.UpdateMainCamera();
        });
    }
    private void OnClickBack()
    {
        AudioManager.Instance.PlaySFX("btn1");
        SceneLoaderNew.i.LoadScene("Home UI");
    }

    private void Reset()
    {

    }
}
