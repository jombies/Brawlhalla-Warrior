using System;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public LevelUpgradeData upgradeData;
    private void Start()
    {
        this.RegisterListener(EventID.GetUpgradeData, OnGetUpgradeData);
    }

    private void OnGetUpgradeData(object obj)
    {
        Debug.LogError("Upgrade Data Loaded: " + upgradeData.name);
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
        var stats1 = upgradeData.GetStatsForLevel(player.level + 2);
        this.PostEvent(EventID.UpgradeCostUpdate, stats1.cost);
    }
}
