using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer & Volume")]
    public AudioMixer mixer;
    [Range(0.001f, 1f)] public float masterVolume = 1f;
    [Range(0.001f, 1f)] public float musicVolume = 1f;
    [Range(0.001f, 1f)] public float sfxVolume = 1f;

    [Header("Audio Database")]
    public AudioDatabase audioData;

    //[Header("Audio Groups")]
    //public Sound[] backgroundMusic;
    //public Sound[] combatSounds;
    //public Sound[] environmentSounds;
    //public Sound[] uiSounds;
    //public Sound[] playerSounds;

    private Dictionary<string, SoundData> allSounds = new Dictionary<string, SoundData>();
    private AudioSource currentMusicSource;
    private Coroutine fadeRoutine;
    private int field;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        masterVolume = PlayerPrefs.GetFloat($"{EventID.MasterVolume}", 0.8f);
        musicVolume = PlayerPrefs.GetFloat($"{EventID.MusicVolume}", 0.6f);
        sfxVolume = PlayerPrefs.GetFloat($"{EventID.SfxVolume}", 0.6f);
        InitAllSounds();
        ApplyVolume();
    }
    void Update()
    {
        ApplyVolume(); // Optional: real-time volume control
    }

    void InitAllSounds()
    {
        InitSoundArray(audioData.backgroundMusic, "Music");
        InitSoundArray(audioData.combatSounds, "SFX");
        InitSoundArray(audioData.environmentSounds, "SFX");
        InitSoundArray(audioData.uiSounds, "SFX");
        InitSoundArray(audioData.playerSounds, "SFX");
    }

    void InitSoundArray(SoundData[] sounds, string mixerGroup)
    {
        foreach (var s in sounds) {
            GameObject go = new GameObject("Sound_" + s.name);
            go.transform.parent = transform;

            AudioSource src = go.AddComponent<AudioSource>();
            src.clip = s.clip;
            src.loop = s.loop;
            src.pitch = s.pitch;
            src.volume = s.volume;
            src.playOnAwake = false;

            if (mixer != null) {
                var group = mixer.FindMatchingGroups(mixerGroup);
                if (group.Length > 0)
                    src.outputAudioMixerGroup = group[0];
                else
                    Debug.LogWarning($"[AudioManager] Mixer group '{mixerGroup}' not found. Check spelling.");
            }

            s.source = src;
            allSounds[s.name] = s;
        }
    }

    // -----------------------------------
    // Sound Methods
    // -----------------------------------
    public void PlaySFX(string name)
    {
        if (allSounds.TryGetValue(name, out SoundData s)) {
            s.source.PlayOneShot(s.clip, s.volume * sfxVolume);
        }
    }

    public void PlayLooped(string name)
    {
        if (allSounds.TryGetValue(name, out SoundData s)) {
            s.source.loop = true;
            s.source.Play();
        }
    }

    public void StopSound(string name)
    {
        if (allSounds.TryGetValue(name, out SoundData s)) {
            s.source.Stop();
        }
    }

    // -----------------------------------
    // Music Methods with Fade
    // -----------------------------------
    public void PlayMusic(string name, float fadeDuration = 0.5f)
    {
        if (!allSounds.ContainsKey(name)) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeToMusic(allSounds[name], fadeDuration));
    }

    IEnumerator FadeToMusic(SoundData newMusic, float duration)
    {
        if (currentMusicSource != null) {
            yield return StartCoroutine(FadeOut(currentMusicSource, duration));
            currentMusicSource.Stop();
        }

        currentMusicSource = newMusic.source;
        currentMusicSource.volume = 0f;
        currentMusicSource.Play();

        yield return StartCoroutine(FadeIn(currentMusicSource, duration, newMusic.volume * musicVolume));
    }

    IEnumerator FadeOut(AudioSource src, float time)
    {
        float start = src.volume;
        float t = 0f;

        while (t < time) {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0f, t / time);
            yield return null;
        }

        src.volume = 0f;
    }

    IEnumerator FadeIn(AudioSource src, float time, float targetVol)
    {
        float t = 0f;
        while (t < time) {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, targetVol, t / time);
            yield return null;
        }

        src.volume = targetVol;
    }

    // -----------------------------------
    // Volume Control
    // -----------------------------------
    void ApplyVolume()
    {
        SetMixerVolume("Master", masterVolume);
        SetMixerVolume("music", musicVolume);
        SetMixerVolume("SFX", sfxVolume);
    }

    void SetMixerVolume(string exposedParam, float vol)
    {
        float dB = vol <= 0.0001f ? -80f : Mathf.Log10(vol) * 20f;
        mixer.SetFloat(exposedParam, dB);
    }
}