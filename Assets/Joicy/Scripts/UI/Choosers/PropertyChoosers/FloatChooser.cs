using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatChooser : PropertyChooser
{
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_Text valueDisplay = null;

    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);
        slider.value = (float)Property.GetValue(Object);
    }

    public void ApplyValue(float value)
    {
        if (Object != null)
        {
            Property.SetValue(Object, value);
            valueDisplay.text = value.ToString("0.00");
        }
    }
}
