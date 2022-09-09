using UnityEngine;

public class AudioSettings : Settings
{
    public AudioSettings()
    {
        GlobalVolume = 1.0f;
        AmbientVolume = 1.0f;
        MusicVolume = 1.0f;
        EffectsVolume = 1.0f;
        DroneEngineVolume = 1.0f;
    }

    [SerializeField] public float GlobalVolume { get; set; }
    [SerializeField] public float AmbientVolume { get; set; }
    [SerializeField] public float MusicVolume { get; set; }
    [SerializeField] public float EffectsVolume { get; set; }
    [SerializeField] public float DroneEngineVolume { get; set; }
}