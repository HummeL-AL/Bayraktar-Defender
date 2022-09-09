using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BoolChooser : PropertyChooser
{
    [SerializeField] protected Toggle toggle = null;

    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);
        toggle.isOn = (bool)Property.GetValue(Object);

    }
    public void ApplyValue(bool value)
    {
        if (Object != null)
        {
            Property.SetValue(Object, value);
        }
    }
}
