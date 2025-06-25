using UnityEngine;

[System.Serializable]
public class UpgradeStats
{
    public int level;
    public int hpIncrease;
    public int armorIncrease;
    public int attackIncrease;
    public int cost;
}

[CreateAssetMenu(fileName = "LevelUpgradeData", menuName = "ScriptableObjects/LevelUpgradeData")]
public class LevelUpgradeData : ScriptableObject
{
    public UpgradeStats[] upgrades;

    public UpgradeStats GetStatsForLevel(int level)
    {
        foreach (var stats in upgrades) {
            if (stats.level == level)
                return stats;
        }

        Debug.Log("Không tìm thấy nâng cấp cho cấp độ: " + level);
        return null;
    }
}
