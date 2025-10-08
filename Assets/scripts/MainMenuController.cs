using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Scene Settings")]
    public string gameSceneName = "GameScene"; // Name of your game scene

    void Start()
    {
        // Load saved volume settings
        LoadVolumeSettings();

        // Setup slider events
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void PlayGame()
    {
        // Load the game scene (scene index 1)
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        // Save settings before quitting
        SaveVolumeSettings();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetMusicVolume(float volume)
    {
        // Convert linear 0-1 value to logarithmic dB scale
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat("MusicVolume", dB);

        // Save the setting
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Convert linear 0-1 value to logarithmic dB scale
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        audioMixer.SetFloat("SFXVolume", dB);

        // Save the setting
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolumeSettings()
    {
        // Load music volume (default to 0.7 if not set)
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            SetMusicVolume(musicVolume);
        }

        // Load SFX volume (default to 0.7 if not set)
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            SetSFXVolume(sfxVolume);
        }
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        SaveVolumeSettings();
    }
}