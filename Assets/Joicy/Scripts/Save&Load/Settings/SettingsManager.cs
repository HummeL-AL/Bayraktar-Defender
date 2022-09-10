using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannel settingsChangedChannel = null;
    
    [SerializeField] private AudioMixer audioMixer = null;

    [Inject] private SettingsData settingsData = null;
    private Settings[] settingsTypes = null;

    public void SaveSettings()
    {
        foreach (Settings settings in settingsTypes)
        {
            foreach (PropertyInfo setting in settingsData.GetSettingsProperties(settings))
            {
                string type = setting.PropertyType.Name;
                object value = setting.GetValue(settings, null);

                SaveValue(setting.Name, type, value);
            }
        }

        settingsChangedChannel.RaiseEvent();
    }

    public void LoadSettings()
    {
        foreach (Settings settings in settingsTypes)
        {
            foreach (PropertyInfo setting in settingsData.GetSettingsProperties(settings))
            {
                string name = setting.Name;
                string type = setting.PropertyType.Name;

                LoadValue(name, type, out object value);
                setting.SetValue(settings, value);
            }
        }

        settingsChangedChannel.RaiseEvent();
    }

    private void Awake()
    {
        settingsChangedChannel.ChannelEvent += ApplySettings;

        InitializeTypes();
        LoadSettings();
    }

    private void InitializeTypes()
    {
        settingsTypes = settingsData.SettingsTypes;
    }

    private void SaveValue(string name, string type, object rawValue)
    {
        switch (type)
        {
            case "Boolean":
                {
                    int value = (bool)rawValue ? 1 : 0;
                    PlayerPrefs.SetInt(name, value);

                    break;
                }
            case "Single":
                {
                    float value = (float)rawValue;
                    PlayerPrefs.SetFloat(name, value);

                    break;
                }
            case "Resolution":
                {
                    Resolution resolution = (Resolution)rawValue;
                    string value = $"{resolution.width}x{resolution.height}";
                    PlayerPrefs.SetString(name, value);
                    break;
                }
            case "FullScreenMode":
                {
                    int value = (int)rawValue;
                    PlayerPrefs.SetInt(name, value);
                    break;
                }
            case "Int32":
                {
                    int value = (int)rawValue;
                    PlayerPrefs.SetInt(name, value);
                    break;
                }
            default:
                {
                    Debug.Log($"{name} has unknown type: {type}");

                    break;
                }
        }
    }

    private void LoadValue(string name, string type, out object value)
    {
        switch (type)
        {
            case "Boolean":
                {
                    value = Convert.ToBoolean(PlayerPrefs.GetInt(name));

                    break;
                }
            case "Single":
                {
                    value = PlayerPrefs.GetFloat(name);

                    break;
                }
            case "Resolution":
                {

                    Resolution[] resolutions = Screen.resolutions;
                    value = resolutions[resolutions.Length - 1];

                    string[] currentResolution = PlayerPrefs.GetString(name).Split('x');
                    int currentWidth = Convert.ToInt32(currentResolution[0]);
                    int currentHeight = Convert.ToInt32(currentResolution[1]);

                    for (int i = 0; i < resolutions.Length; i++)
                    {
                        if (resolutions[i].width == currentWidth && resolutions[i].height == currentHeight)
                        {
                            value = resolutions[i];
                            break;
                        }
                    }
                    break;
                }
            case "FullScreenMode":
                {
                    value = (FullScreenMode)PlayerPrefs.GetInt(name);
                    break;
                }
            case "Int32":
                {
                    value = PlayerPrefs.GetInt(name);
                    break;
                }
            default:
                {
                    Debug.Log($"{name} has unknown type: {type}");
                    value = null;

                    break;
                }
        }
    }

    private void ApplySettings()
    {
        ApplyGraphicSettings();
        ApplyAudioSettings();
    }

    private void ApplyGraphicSettings()
    {
        GraphicSettings graphicSettings = settingsData.Graphic;

        Resolution targetResolution = graphicSettings.ScreenResolution;
        Screen.SetResolution(targetResolution.width, targetResolution.height, graphicSettings.ScreenMode);
        QualitySettings.SetQualityLevel(graphicSettings.GraphicPreset);
    }

    private void ApplyAudioSettings()
    {
        AudioSettings audioSettings = settingsData.Audio;

        ApplyVolumeLevel("masterVolume", audioSettings.GlobalVolume);
        ApplyVolumeLevel("ambientSoundsVolume", audioSettings.AmbientVolume);
        ApplyVolumeLevel("musicVolume", audioSettings.MusicVolume);
        ApplyVolumeLevel("gameEffectsVolume", audioSettings.EffectsVolume);
        ApplyVolumeLevel("droneEngineVolume", audioSettings.DroneEngineVolume);
    }

    private void ApplyVolumeLevel(string name, float value)
    {
        float volumeLevel = Mathf.Lerp(-80f, 0f, value);
        audioMixer.SetFloat(name, volumeLevel);
    }
}