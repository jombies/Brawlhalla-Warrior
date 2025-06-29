using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public static SettingUI Instance { get; private set; }
    [SerializeField] public Transform settingPanel;
    [SerializeField] private Button btnClose;
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider MusicVolSlider;
    [SerializeField] private Slider SfxVolSlider;
    public bool isSetting = false;
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        btnClose.onClick.AddListener(OnClickClose);
        masterVolSlider.onValueChanged.AddListener(OnChangeMasterVolume);
        MusicVolSlider.onValueChanged.AddListener(OnChangeMusicVolume);
        SfxVolSlider.onValueChanged.AddListener(OnChangeSfxVolume);
        initializeVolume();
    }

    void initializeVolume()
    {
        if (PlayerPrefs.HasKey($"{EventID.MasterVolume}")) {
            masterVolSlider.value = PlayerPrefs.GetFloat($"{EventID.MasterVolume}");
        }
        else {
            masterVolSlider.value = 0.8f;
        }
        if (PlayerPrefs.HasKey($"{EventID.MusicVolume}")) {
            MusicVolSlider.value = PlayerPrefs.GetFloat($"{EventID.MusicVolume}");
        }
        else {
            MusicVolSlider.value = 0.6f;
        }
        if (PlayerPrefs.HasKey($"{EventID.SfxVolume}")) {
            SfxVolSlider.value = PlayerPrefs.GetFloat($"{EventID.SfxVolume}");
        }
        else {
            SfxVolSlider.value = 0.6f;
        }
    }

    public void ShowSettingPanel()
    {
        settingPanel.gameObject.SetActive(true);
        isSetting = true;
    }
    private void OnChangeMasterVolume(float arg0)
    {
        PlayerPrefs.SetFloat($"{EventID.MasterVolume}", arg0);
        PlayerPrefs.Save();
        AudioManager.Instance.masterVolume = arg0;
    }
    private void OnChangeMusicVolume(float arg0)
    {
        PlayerPrefs.SetFloat($"{EventID.MusicVolume}", arg0);
        PlayerPrefs.Save();
        AudioManager.Instance.musicVolume = arg0;
    }
    private void OnChangeSfxVolume(float arg0)
    {
        PlayerPrefs.SetFloat($"{EventID.SfxVolume}", arg0);
        PlayerPrefs.Save();
        AudioManager.Instance.sfxVolume = arg0;
    }

    private void OnClickClose()
    {
        AudioManager.Instance.PlaySFX("btn1");
        settingPanel.gameObject.SetActive(false);
        isSetting = false;
    }

    private void Reset()
    {
        settingPanel = transform.Find("ScreenDim");
        Transform popup = settingPanel.Find("Popup");
        btnClose = popup.Find("Button_Close").GetComponent<Button>();
        Transform groupSetting = popup.Find("Group_Setting");
        Transform MasterVolumre = groupSetting.Find("MasterVolumre");
        masterVolSlider = MasterVolumre.Find("SliderBar").GetComponent<Slider>();
        Transform Music = groupSetting.Find("Music");
        MusicVolSlider = Music.Find("SliderBar").GetComponent<Slider>();
        Transform Sfx = groupSetting.Find("Sfx");
        SfxVolSlider = Sfx.Find("SliderBar").GetComponent<Slider>();
    }
}
