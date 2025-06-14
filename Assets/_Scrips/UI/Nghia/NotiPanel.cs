using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotiPanel : MonoBehaviour
{
    public static NotiPanel Instance { get; private set; }
    [SerializeField] GameObject _container;
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _contentText;
    [SerializeField] Button _yesBtn;
    public Button YesBtn { get => _yesBtn; }
    [SerializeField] Button _noBtn;
    public Button NoBtn { get => _noBtn; }
    UnityEvent _onYes, _onNo;
    protected void Awake()
    {
        _onYes = new UnityEvent();
        _onNo = new UnityEvent();
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _yesBtn.onClick.AddListener(Yes);
        _noBtn.onClick.AddListener(No);
    }
    public void ShowNotify(string title, string noti, UnityAction onYes, UnityAction onNo = null)
    {
        _container.SetActive(true);
        _titleText.text = title;
        _contentText.text = noti;
        _onYes.RemoveAllListeners();
        _onNo.RemoveAllListeners();
        if (onYes != null) {
            _onYes.AddListener(onYes);
        }
        if (onNo != null) {
            _onNo.AddListener(onNo);
        }
    }
    public void Yes()
    {
        AudioManager.Instance.PlaySFX("btn1");
        _onYes?.Invoke();
        _onYes?.RemoveAllListeners();
        _onNo?.RemoveAllListeners();
        _container.SetActive(false);
    }
    public void No()
    {
        AudioManager.Instance.PlaySFX("btn1");
        _onNo?.Invoke();
        _onYes?.RemoveAllListeners();
        _onNo?.RemoveAllListeners();
        _container.SetActive(false);
    }
    private void OnDestroy()
    {
        _yesBtn.onClick.RemoveListener(Yes);
        _noBtn.onClick.RemoveListener(No);
    }
    private void Reset()
    {
        _container = transform.Find("Container").gameObject;
        _titleText = _container.transform.Find("Text_Title").GetComponent<TextMeshProUGUI>();
        _contentText = _container.transform.Find("Text_Info").GetComponent<TextMeshProUGUI>();
        Transform buttons = _container.transform.Find("Buttons");
        _yesBtn = buttons.transform.Find("Button_OK").GetComponent<Button>();
        _noBtn = buttons.transform.Find("Button_Cancel").GetComponent<Button>();
    }

}
