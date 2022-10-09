using UnityEngine;

public class AnglesDisplay : InfoDisplay
{
    [SerializeField] private Transform _object = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        Vector3 rotation = _object.localEulerAngles;
        string xRotation = string.Format("{0:0}", rotation.x);
        string yRotation = string.Format("{0:0}", rotation.y);
        display.text = $"ANG:{xRotation}°{yRotation}°";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
