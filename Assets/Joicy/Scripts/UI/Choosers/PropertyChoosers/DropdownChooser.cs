using System.Reflection;
using UnityEngine;
using TMPro;

public class DropdownChooser : PropertyChooser
{
    [SerializeField] protected TMP_Dropdown dropdown;

    public override void Initialize(PropertyInfo property, object targetObject)
    {
        base.Initialize(property, targetObject);
    }
}
