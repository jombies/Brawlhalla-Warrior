using UnityEngine;
using DG.Tweening;

public class PopupAnimator : MonoBehaviour
{
    [SerializeField] RectTransform popupTransform;
    [SerializeField] float moveDistance = 600f;
    [SerializeField] float duration = 1f;
    [SerializeField] Ease moveEase = Ease.OutBack;

    private Vector2 targetPosition;

    void Awake()
    {
        if (popupTransform == null)
            popupTransform = GetComponent<RectTransform>();

        targetPosition = popupTransform.anchoredPosition;
    }

    public void ShowFromTop()
    {
        popupTransform.anchoredPosition = targetPosition + new Vector2(0, moveDistance);
        popupTransform.DOAnchorPos(targetPosition, duration).SetEase(moveEase).SetUpdate(true);
    }

    public void ShowFromBottom()
    {
        popupTransform.anchoredPosition = targetPosition - new Vector2(0, moveDistance);
        popupTransform.DOAnchorPos(targetPosition, duration).SetEase(moveEase).SetUpdate(true);
    }
}
