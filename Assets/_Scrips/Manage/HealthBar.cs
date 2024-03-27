using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider Slider;
    public Gradient gradient;
    public Image image;
    // Start is called before the first frame update
    void Awake()
    {
        Slider = GetComponent<Slider>();
        image = GetComponentInChildren<Image>();
    }

    public void SetMaxHeathBar(int health)
    {
        Slider.maxValue = health;
        Slider.value = health;
        image.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        Slider.value = health;
        image.color = gradient.Evaluate(Slider.normalizedValue);
    }
}
