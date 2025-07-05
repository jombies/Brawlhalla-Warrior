using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    static GamePlayUI Instance;
    [Header("UI Panels")]
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject pausePanel;

    [Header("UI Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button settingButton;

    [Header("conditions")]
    public bool isPause = false;
    [SerializeField] bool isQuit = false;
    [SerializeField] bool isSetting = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // tránh đè
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ chuột khi bắt đầu
        pauseButton.onClick.AddListener(OnPause);
        resumeButton.onClick.AddListener(OnResume);
        settingButton.onClick.AddListener(OnSetting);
        homeButton.onClick.AddListener(OnHome);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void Update()
    {
        isSetting = SettingUI.Instance.isSetting;
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.victoryPopup.activeSelf && !GameManager.Instance.defeatPopup.activeSelf) {
            if (isSetting) return;
            if (pausePanel.activeSelf) {
                OnResume();
            }
            else {
                OnPause();
            }
        }

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[GamePlayUI] Scene Loaded: " + scene.name);
        TryReassignUI();
    }
    void TryReassignUI()
    {
        if (pauseButton == null) {
            Debug.Log("[GamePlayUI] Rebinding UI components...");
            Reset(); // gán lại từ GameObject.Find(...)
            pauseButton?.onClick.AddListener(OnPause);
            resumeButton?.onClick.AddListener(OnResume);
            settingButton?.onClick.AddListener(OnSetting);
            homeButton?.onClick.AddListener(OnHome);
            quitButton?.onClick.AddListener(OnQuit);
        }
    }

    void OnPause()
    {
        isPause = true;
        AudioManager.Instance.PlaySFX("btn1");
        GamePanel.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; // Mở khóa con trỏ chuột khi pause
    }
    void OnResume()
    {
        AudioManager.Instance.PlaySFX("btn1");
        GamePanel.SetActive(true);
        pausePanel.SetActive(false);
        isPause = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ chuột khi bắt đầu
    }
    void OnSetting()
    {
        AudioManager.Instance.PlaySFX("btn1");
        SettingUI.Instance.ShowSettingPanel();
    }
    void OnQuit()
    {
        AudioManager.Instance.PlaySFX("btn1");
        NotiPanel notiPanel = NotiPanel.Instance;
        notiPanel.ShowNotify("Thoát Game", "Bạn muốn thoát game?", () => { Application.Quit(); }, () => { });
    }
    void OnHome()
    {
        Time.timeScale = 1;
        AudioManager.Instance.PlaySFX("btn1");
        SceneLoaderNew.i.LoadScene("Home UI", () => GameManager.Instance.CleanUpSystemsForHome());
        pausePanel.SetActive(false);
        GamePanel.SetActive(true);
    }

    private void Reset()
    {
        GamePanel = GameObject.Find("GamePlay");
        pausePanel = GameObject.Find("PauseMenu");
        pauseButton = GamePanel.transform.GetChild(1).Find("PauseButton").GetComponent<Button>();
        resumeButton = pausePanel.transform.GetChild(1).Find("Button_Continue").GetComponent<Button>();
        homeButton = pausePanel.transform.GetChild(1).Find("Button_Home").GetComponent<Button>();
        quitButton = pausePanel.transform.GetChild(1).Find("Button_Quit").GetComponent<Button>();
        settingButton = pausePanel.transform.GetChild(1).Find("Button_Setting").GetComponent<Button>();
    }

}
