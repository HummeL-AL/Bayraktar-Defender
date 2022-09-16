using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioEffect_", menuName = "ScriptableObjects/Audio", order = 1)]
public class AudioEffect : ScriptableObject
{
    [SerializeField] private AudioClip audio = null;
    [SerializeField] private AudioMixerGroup mixer = null;
    [SerializeField] private Vector2 volume = Vector2.one;
    [SerializeField] private Vector2 pitch = Vector2.one;
    [SerializeField] private Vector2 distance = Vector2.up;

    public AudioClip Clip { get => audio; }
    public AudioMixerGroup MixerGroup { get => mixer; }
    public float Volume { get => Random.Range(volume.x, volume.y); }
    public float Pitch { get => Random.Range(pitch.x, pitch.y); }
    public Vector2 Distance { get => distance; }
}
