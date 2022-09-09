using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class IntChooser : DropdownChooser
{
    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);
        List<string> options = new List<string>();
        foreach (string option in QualitySettings.names)
        {
            options.Add(option);
        }

        dropdown.AddOptions(options);
        dropdown.value = (int)Property.GetValue(Object);
    }

    public void ApplyValue(int index)
    {
        if (Object != null)
        {
            Property.SetValue(Object, index);
        }
    }
}