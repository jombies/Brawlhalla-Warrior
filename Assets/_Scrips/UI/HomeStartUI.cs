using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeStartUI : MonoBehaviour
{
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

        coinText.text = $"{data.coins}";
        healthSlider.value = data.maxHP;
        armorSlider.value = data.armor;
        attackSlider.value = data.attack;
        healthText.text = $"{data.maxHP}";
        armorText.text = $"{data.armor}";
    }

    private void OnClickUpgrade()
    {
        this.PostEvent(EventID.GetUpgradeData);
        Debug.Log("Upgrade button clicked.");
    }
    private void OnClickPlay()
    {
        AudioManager.Instance.PlaySFX("btnStart");
        GameManager.Instance.PrepareDataBeforePlay();
        // SceneLoader.i.loadScene("Testing");
        SceneLoaderNew.i.loadScene("Testing", () =>
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
        SceneLoaderNew.i.loadScene("Home UI");
    }

    private void Reset()
    {

    }
}
