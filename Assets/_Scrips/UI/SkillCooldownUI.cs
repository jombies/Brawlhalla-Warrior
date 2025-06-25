using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] GameObject dashButton;
    public Image dashImage;
    public float dashCooldownTime = 2f;
    private float dashTimer = 0f;

    [Header("Power")]
    [SerializeField] GameObject powerButton;
    public Image powerImage;
    public float powerCooldownTime = 5f;
    private float powerTimer = 0f;
    public int unlockPowerLevel = 2; // mở khóa power ở level mấy

    [Header("Player")]
    public int playerLevel;

    private void Start()
    {
        playerLevel = GameManager.Instance.LoadedData.level;
    }
    void Update()
    {
        // Cập nhật cooldown Dash
        if (dashTimer > 0f) {
            dashTimer -= Time.deltaTime;
            dashImage.fillAmount = dashTimer / dashCooldownTime;
        }

        // Cập nhật cooldown Power
        if (powerTimer > 0f) {
            powerTimer -= Time.deltaTime;
            powerImage.fillAmount = powerTimer / powerCooldownTime;
        }

        if (playerLevel < unlockPowerLevel) {
            powerButton.SetActive(false);
        }
        else {
            powerButton.SetActive(true);
        }
    }

    public void StartDashCooldown()
    {
        dashTimer = dashCooldownTime;
        dashImage.fillAmount = 1f;
    }

    public void StartPowerCooldown()
    {
        if (playerLevel >= unlockPowerLevel) {
            powerTimer = powerCooldownTime;
            powerImage.fillAmount = 1f;
        }
    }

    public bool IsDashReady()
    {
        return dashTimer <= 0f;
    }

    public bool IsPowerReady()
    {
        return powerTimer <= 0f && playerLevel >= unlockPowerLevel;
    }
}
