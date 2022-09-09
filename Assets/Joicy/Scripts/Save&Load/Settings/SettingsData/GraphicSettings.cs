using UnityEngine;

public class GraphicSettings : Settings
{
    public GraphicSettings()
    {
        ScreenResolution = new Resolution();
        ScreenMode = FullScreenMode.FullScreenWindow;
        GraphicPreset = 3;
        CameraNoise = true;
    }

    [SerializeField] public Resolution ScreenResolution { get; set; }
    [SerializeField] public FullScreenMode ScreenMode { get; set; }
    [SerializeField] public int GraphicPreset { get; set; }
    [SerializeField] public bool CameraNoise { get; set; }
}
