using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    void Awake()
    {
        // Singleton pattern to keep audio manager across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load volume settings when audio manager is created
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadVolumeSettings()
    {
        // Load music volume
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        float musicdB = musicVolume > 0 ? 20f * Mathf.Log10(musicVolume) : -80f;
        audioMixer.SetFloat("MusicVolume", musicdB);

        // Load SFX volume
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        float sfxdB = sfxVolume > 0 ? 20f * Mathf.Log10(sfxVolume) : -80f;
        audioMixer.SetFloat("SFXVolume", sfxdB);
    }
}