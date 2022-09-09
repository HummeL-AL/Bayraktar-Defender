using UnityEngine;

public class ControlSettings : Settings
{
    public ControlSettings()
    {
        Sensitivity = 1.0f;
    }

    [SerializeField] public float Sensitivity { get; set; }
}
