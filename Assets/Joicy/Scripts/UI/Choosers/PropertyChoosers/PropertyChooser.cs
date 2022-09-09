using System.Reflection;
using UnityEngine;
using TMPro;

public abstract class PropertyChooser : MonoBehaviour
{
    protected PropertyInfo Property { get; set; }
    protected object Object { get; set; }

    [SerializeField] protected TMP_Text nameDisplay = null;

    public virtual void Initialize(PropertyInfo property, object targetObject)
    {
        Property = property;
        Object = targetObject;
        UpdateText();
    }

    public void UpdateText()
    {
        nameDisplay.text = Property.Name;
    }
}