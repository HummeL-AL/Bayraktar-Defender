using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionChooser : DropdownChooser
{
    private Resolution[] resolutions = null;

    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);
        string currentResolution = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";

        List<string> options = new List<string>();
        foreach(Resolution resolution in Screen.resolutions)
        {
            options.Add($"{resolution.width}x{resolution.height}");
        }
        int index = options.IndexOf(currentResolution);
        dropdown.AddOptions(options);
        dropdown.value = index;
    }

    public void ApplyValue(int index)
    {
        if (Object != null)
        {
            Resolution value = resolutions[index];
            Property.SetValue(Object, value);
        }
    }

    private void Awake()
    {
        resolutions = Screen.resolutions;
    }
}
