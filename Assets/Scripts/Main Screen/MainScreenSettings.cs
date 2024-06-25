using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainScreenSettings : MonoBehaviour
{
    private const string MUSIC_VOLUME = "Music Volume";
    private const string SFX_VOLUME = "SFX Volume";

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_InputField inputDelayField;

    [SerializeField] private AudioMixer MusicMixer;
    [SerializeField] private AudioMixer SFXMixer;

    private float volume;
    private float sfxVolume;
    private int qualityIndex;
    private int inputDelay;

    private string settingsFilePath;

    private void Start()
    {
        settingsFilePath = Application.persistentDataPath + "/settings.json";
        LoadSettings();
    }

    public void SetVolume(float volume)
    {
        MusicMixer.SetFloat(MUSIC_VOLUME, volume);
        this.volume = volume;
    }

    public void SetSFXVolume(float sfxVolume)
    {
        SFXMixer.SetFloat(SFX_VOLUME, sfxVolume);
        this.sfxVolume = sfxVolume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        this.qualityIndex = qualityIndex;
    }

    public void SetInputDelay()
    {
        // Parse input delay value from the input field, handling empty input
        if (string.IsNullOrEmpty(inputDelayField.text))
        {
            inputDelay = 0;
        }
        else
        {
            if (int.TryParse(inputDelayField.text, out int parsedInputDelay))
            {
                inputDelay = parsedInputDelay;
            }
            else
            {
                Debug.LogWarning("Invalid input delay value. Setting input delay to 0.");
                inputDelay = 0;
            }
        }
    }

    public void SaveSettings()
    {
        SetInputDelay();
        SettingData data = new SettingData(volume, sfxVolume, qualityIndex, inputDelay);
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(settingsFilePath, json);
    }

    public void LoadSettings()
    {
        if (System.IO.File.Exists(settingsFilePath))
        {
            string json = System.IO.File.ReadAllText(settingsFilePath);
            SettingData data = JsonUtility.FromJson<SettingData>(json);
            volumeSlider.value = data.musicVolume;
            sfxVolumeSlider.value = data.sfxVolume;
            qualityDropdown.value = data.graphicsQuality;
            inputDelayField.text = data.inputDelay.ToString();
            SetVolume(data.musicVolume);
            SetSFXVolume(data.sfxVolume);
            SetQuality(data.graphicsQuality);
            SetInputDelay();
        }
    }
}
