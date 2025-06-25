using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    [SerializeField] RectTransform popupTransform;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float moveDistance = 100f;
    [SerializeField] float duration = 1.5f;

    private Vector2 startPos;

    void Awake()
    {
        startPos = popupTransform.anchoredPosition;
    }
    public void ShowPopup()
    {
        popupTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0;
        popupTransform.gameObject.SetActive(true);

        Sequence s = DOTween.Sequence();

        s.Append(canvasGroup.DOFade(1f, 0.2f))
         .Join(popupTransform.DOAnchorPosY(startPos.y + moveDistance, duration).SetEase(Ease.OutCubic))
         .Append(canvasGroup.DOFade(0f, 0.5f))
         .AppendCallback(() =>
         {
             popupTransform.gameObject.SetActive(false);
         });
    }
    private void Reset()
    {
        popupTransform = transform.GetChild(0).GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
