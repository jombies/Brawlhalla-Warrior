using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;

    [SerializeField] PlayerStat PlayerStat;
    public Gradient gradient;
    // Start is called before the first frame update
    void Start()
    {
        this.RegisterListener(EventID.OnUseItem, (e) => UpdateMaxvalue());
        PlayerStat = PlayerReferences.Instance.Player.GetComponent<PlayerStat>();
    }
    public void SetHealth(int health)
    {
        healthSlider.value = health;
        healthSlider.gameObject.transform.GetComponentInChildren<Image>().color = gradient.Evaluate(healthSlider.normalizedValue);
        // image.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
    public void SetShield(int shield)
    {
        shieldSlider.value = shield;
        shieldSlider.gameObject.transform.GetComponentInChildren<Image>().color = gradient.Evaluate(healthSlider.normalizedValue);
        //image.color = gradient.Evaluate(shieldSlider.normalizedValue);
    }

    public void Updatevalue()
    {
        healthSlider.value = PlayerStat.currentHealth;
        shieldSlider.value = PlayerStat.currentArmor;
    }
    public void UpdateMaxvalue()
    {
        healthSlider.maxValue = PlayerStat.MaxHealth;
        shieldSlider.maxValue = PlayerStat.Armor.Value;
    }
    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnUseItem, (e) => UpdateMaxvalue());
    }
}
