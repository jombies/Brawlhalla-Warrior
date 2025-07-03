using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Audio/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    [Header("Background Music")]
    public SoundData[] backgroundMusic;

    [Header("Combat Sounds")]
    public SoundData[] combatSounds;

    [Header("Environment Sounds")]
    public SoundData[] environmentSounds;

    [Header("UI Sounds")]
    public SoundData[] uiSounds;

    [Header("Player Sounds")]
    public SoundData[] playerSounds;
}
[System.Serializable]
public class SoundData
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;

    [System.NonSerialized]
    public AudioSource source; // ✅ Chỉ dùng khi chạy game, không lưu trong asset
}