using System.IO;
using UnityEngine;

public static class PlayerDataManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "PlayerData.json");

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Đã lưu dữ liệu tại: " + SavePath);
    }

    public static PlayerData Load()
    {
        if (File.Exists(SavePath)) {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }
        else {
            Debug.Log("Không tìm thấy file lưu, tạo dữ liệu mặc định.");
            PlayerData defaultData = new PlayerData { level = 0, maxHP = 100, armor = 6, attack = 6, coins = 100000 };
            Save(defaultData);
            return defaultData;
        }
    }

    public static void ResetData()
    {
        if (File.Exists(SavePath)) {
            File.Delete(SavePath);
            Debug.Log("Đã xóa dữ liệu.");
        }
    }
}
