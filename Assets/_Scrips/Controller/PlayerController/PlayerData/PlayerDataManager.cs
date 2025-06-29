using System.IO;
using UnityEngine;

public static class PlayerDataManager
{
    private static string PlayerData => Path.Combine(Application.persistentDataPath, "PlayerData.json");

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(PlayerData, json);
        Debug.Log("Đã lưu dữ liệu tại: " + PlayerData);
    }

    public static PlayerData Load()
    {
        if (File.Exists(PlayerData)) {
            string json = File.ReadAllText(PlayerData);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else {
            Debug.Log("Không tìm thấy file lưu, tạo dữ liệu mặc định.");
            PlayerData defaultData = new() { level = 0, maxHP = 100, armor = 6, attack = 6, coins = 100000 };
            Save(defaultData);
            return defaultData;
        }
    }

    public static void ResetData()
    {
        if (File.Exists(PlayerData)) {
            File.Delete(PlayerData);
            Debug.Log("Đã xóa dữ liệu.");
        }
    }
}
