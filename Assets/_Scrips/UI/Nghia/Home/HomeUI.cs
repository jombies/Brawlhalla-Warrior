using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSetting;
    [SerializeField] private Button btnExit;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("musicLobby1");
        btnPlay.onClick.AddListener(OnClickPlay);
        btnSetting.onClick.AddListener(OnClickSetting);
        btnExit.onClick.AddListener(OnClickExit);
    }

    private void OnClickExit()
    {
        NotiPanel notiPanel = NotiPanel.Instance;
        AudioManager.Instance.PlaySFX("btn1");
        if (notiPanel != null) {
            notiPanel.ShowNotify("Exit Game", "Are you sure you want to exit the game?", Application.Quit, () => { });
        }
        else {
            Debug.LogWarning("NotiPanel instance is null. Cannot show exit notification.");
        }
    }

    private void OnClickSetting()
    {
        AudioManager.Instance.PlaySFX("btn1");
        SettingUI.Instance.ShowSettingPanel();

        // SettingUI.Instance.gameObject.SetActive(true); // Uncomment if you want to show the setting UI directly
    }

    private void OnClickPlay()
    {
        AudioManager.Instance.PlaySFX("btn1");
        // SceneLoader.i.loadScene("Information");
        SceneLoaderNew.i.loadScene("Information");
        transform.parent.gameObject.SetActive(false);

    }

    private void Reset()
    {
        btnPlay = transform.Find("Button_Play").GetComponent<Button>();
        Transform groupButtonsMenu = transform.Find("Group_Buttons_Menu");
        Transform setting_Area = groupButtonsMenu.Find("Setting_Area");
        Transform exit_Area = groupButtonsMenu.Find("Exit_Area");
        btnSetting = setting_Area.Find("Button_Settings_Bg").GetComponent<Button>();
        btnExit = exit_Area.Find("Button_Exit_Bg").GetComponent<Button>();
    }
}
