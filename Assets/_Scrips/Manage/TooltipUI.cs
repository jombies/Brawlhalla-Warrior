using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }

    public void Show(Item item, Vector3 pos)
    {
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = pos + new Vector3(250, 0, 0);
        nameText.text = item.name;
        descriptionText.text = item.description;
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
