using System.Reflection;
using UnityEngine;
using Zenject;

public class SettingsDisplay : MonoBehaviour
{
    [SerializeField] private Transform tabsPanel = null;
    [SerializeField] private Transform contentPanel = null;
    [SerializeField] private SettingsTabButton tabButton = null;

    [SerializeField] private BoolChooser boolChooser = null;
    [SerializeField] private FloatChooser floatChooser = null;
    [SerializeField] private ResolutionChooser resolutionChooser = null;
    [SerializeField] private FullScreenModeChooser fullScreenModeChooser = null;
    [SerializeField] private IntChooser intChooser = null;

    [Inject] private SettingsData settingsData = null;
    private Settings[] settingsTypes = null;

    public void SetTabs()
    {
        Clear(tabsPanel);

        for (int i = 0; i < settingsTypes.Length; i++)
        {
            Settings settings = settingsTypes[i];

            SettingsTabButton button = Instantiate(tabButton, tabsPanel);
            button.Initialize(this, i, settings.GetType().Name);
        }
    }

    public void DisplaySettings(int index)
    {
        Clear(contentPanel);

        Settings settings = settingsTypes[index];
        foreach (PropertyInfo setting in settingsData.GetSettingsProperties(settings))
        {
            string type = setting.PropertyType.Name;
            PropertyChooser propertyChooser = GetChooser(type);
            
            if (propertyChooser)
            {
                propertyChooser = Instantiate(propertyChooser, contentPanel);
                propertyChooser.Initialize(setting, settings);
            }
        }
    }

    private void Awake()
    {
        InitializeTypes();
    }

    private void InitializeTypes()
    {
        settingsTypes = settingsData.SettingsTypes;
    }

    private PropertyChooser GetChooser(string type)
    {
        PropertyChooser propertyChooser = null;

        switch (type)
        {
            case "Boolean":
                {
                    propertyChooser = boolChooser;
                    break;
                }
            case "Single":
                {
                    propertyChooser = floatChooser;
                    break;
                }
            case "Resolution":
                {
                    propertyChooser = resolutionChooser;
                    break;
                }
            case "FullScreenMode":
                {
                    propertyChooser = fullScreenModeChooser;
                    break;
                }
            case "Int32":
                {
                    propertyChooser = intChooser;
                    break;
                }
            default:
                {
                    break;
                }
        }

        return propertyChooser;
    }

    private void Clear(Transform targetTransform)
    {
        for (int i = 0; i < targetTransform.childCount; i++)
        {
            Destroy(targetTransform.GetChild(i).gameObject);
        }
    }
}
