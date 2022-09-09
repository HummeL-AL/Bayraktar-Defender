using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class SettingsData
{
    public SettingsData()
    {
        Graphic = new GraphicSettings();
        Audio = new AudioSettings();
        Control = new ControlSettings();
    }

    public SettingsData(GraphicSettings graphic, AudioSettings audio, ControlSettings control)
    {
        Graphic = graphic;
        Audio = audio;
        Control = control;
    }

    [SerializeField] public GraphicSettings Graphic { get; private set; }
    [SerializeField] public AudioSettings Audio { get; private set; }
    [SerializeField] public ControlSettings Control { get; private set; }

    public Settings[] SettingsTypes
    {
        get
        {
            List<Settings> types = new List<Settings>();
            foreach (PropertyInfo settingsType in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (settingsType.IsDefined(typeof(SerializeField), true))
                {
                    Settings settings = settingsType.GetValue(this) as Settings;
                    if (settings != null)
                    {
                        types.Add(settings);
                    }
                }
            }
            return types.ToArray();
        }
    }

    public PropertyInfo[] GetSettingsProperties(Settings settings)
    {
        List<PropertyInfo> properties = new List<PropertyInfo>();
        foreach (PropertyInfo setting in settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (setting.IsDefined(typeof(SerializeField), true))
            {
                properties.Add(setting);
            }
        }

        return properties.ToArray();
    }
}