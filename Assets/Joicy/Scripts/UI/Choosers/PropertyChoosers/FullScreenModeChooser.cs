using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenModeChooser : DropdownChooser
{
    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);

        List<string> options = new List<string>();
        foreach (FullScreenMode option in Enum.GetValues(typeof(FullScreenMode)))
        {
            options.Add(option.ToString());
        }

        FullScreenMode mode = (FullScreenMode)Property.GetValue(Object);
        int index = options.IndexOf(mode.ToString());
        dropdown.AddOptions(options);
        dropdown.value = index;
    }

    public void ApplyValue(int index)
    {
        if (Object != null)
        {
            string name = dropdown.options[index].text;
            FullScreenMode value = (FullScreenMode)Enum.Parse(typeof(FullScreenMode), name);
            Property.SetValue(Object, value);
        }
    }
}
