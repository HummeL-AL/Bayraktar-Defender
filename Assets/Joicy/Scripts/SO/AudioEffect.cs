using UnityEngine;

[CreateAssetMenu(fileName = "AudioEffect_", menuName = "ScriptableObjects/Audio", order = 1)]
public class AudioEffect : ScriptableObject
{
    [SerializeField] private AudioClip _audio = null;
    [SerializeField] private Vector2 _volume = Vector2.one;
    [SerializeField] private Vector2 _pitch = Vector2.one;

    public void PlayAudioClip(Vector3 position)
    {
        float volume = Random.Range(_volume.x, _volume.y);
        AudioSource.PlayClipAtPoint(_audio, position, volume);
    }
}
