using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartLoader : MonoBehaviour
{
    [SerializeField] Image _loadingBar;
    [SerializeField] float _loadDelay = 3f; // Thời gian giả lập loading (giây)
    [SerializeField] float _fakePauseAt = 0.75f; // Dừng tại 75% progress
    [SerializeField] TextMeshProUGUI _textLoading;

    private void Start()
    {
        StartCoroutine(FakeLoadProgress());
    }

    IEnumerator FakeLoadProgress()
    {
        float timer = 0f;
        bool pausedAtFakePoint = false;
        SceneLoaderNew.i.LoadScene("Home UI");
        while (timer < _loadDelay) {
            // Tính toán progress (0 -> 1)
            float progress = Mathf.Clamp01(timer / _loadDelay);

            // Dừng giả lập tại mốc _fakePauseAt
            if (progress >= _fakePauseAt && !pausedAtFakePoint) {
                pausedAtFakePoint = true;
                yield return new WaitForSeconds(1f); // Dừng 1 giây
            }

            // Cập nhật UI
            _loadingBar.fillAmount = progress;
            if (_textLoading != null)
                _textLoading.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

            timer += Time.deltaTime;
            yield return null;
        }

        // Hoàn thành -> Gọi hàm chuyển scene thực sự

        Destroy(gameObject); // Hủy loader
    }
}