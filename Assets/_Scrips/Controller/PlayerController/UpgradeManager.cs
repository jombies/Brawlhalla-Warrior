using System;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public LevelUpgradeData upgradeData;
    [SerializeField] UpgradePopup upgradePopup;
    private void Start()
    {
        this.RegisterListener(EventID.GetUpgradeData, OnGetUpgradeData);
        UpdateUICost();
    }

    private void UpdateUICost()
    {
        var stat = PlayerDataManager.Load();
        if (stat.level == 6) {
            Debug.Log("level is max");
            return;
        }
        else {
            var costText = upgradeData.GetStatsForLevel(PlayerDataManager.Load().level + 1);
            this.PostEvent(EventID.UpgradeCostUpdate, costText.cost);
        }

    }

    private void OnGetUpgradeData(object obj)
    {
        Debug.Log("Upgrade Data Loaded: " + upgradeData.name);
        UpgradePlayer(PlayerDataManager.Load());
    }

    void UpgradePlayer(PlayerData player)
    {
        var stats = upgradeData.GetStatsForLevel(player.level + 1);
        if (stats == null) return;

        if (player.coins >= stats.cost) {
            player.maxHP += stats.hpIncrease;
            player.armor += stats.armorIncrease;
            player.attack += stats.attackIncrease;
            player.coins -= stats.cost;
            player.level++;

            PlayerDataManager.Save(player);
            AudioManager.Instance.PlaySFX("btnBuy");
            upgradePopup.ShowPopup();
            Debug.Log("Nâng cấp thành công lên cấp " + player.level);
            this.PostEvent(EventID.UpdateFrameUIData);
        }
        else {
            NotiPanel notiPanel = NotiPanel.Instance;
            if (notiPanel != null) {
                notiPanel.ShowNotify("Nâng cấp thất bại", "Bạn không đủ vàng để nâng cấp.", () => { });
            }
            else {
                Debug.LogError("NotiPanel instance is null. Cannot show upgrade failure notification.");
            }
            Debug.Log("Không đủ tiền để nâng cấp.");
        }
        var stats1 = upgradeData.GetStatsForLevel(player.level + 1);
        this.PostEvent(EventID.UpgradeCostUpdate, stats1.cost);
    }
    private void OnDestroy()
    {
        this.RemoveListener(EventID.GetUpgradeData, OnGetUpgradeData);
    }
}
