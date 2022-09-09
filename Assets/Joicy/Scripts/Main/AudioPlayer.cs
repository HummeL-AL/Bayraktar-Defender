using UnityEngine;

public class AudioPlayer
{
    public void PlaySound(AudioEffect effect, Vector3 playPosition)
    {
        GameObject soundContainer = new GameObject("ImpactSound");
        soundContainer.transform.position = playPosition;

        AudioSource audioSource = soundContainer.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.clip = effect.Clip;
        audioSource.outputAudioMixerGroup = effect.MixerGroup;
        audioSource.volume = effect.Volume;
        audioSource.pitch = effect.Pitch;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = effect.Distance.x;
        audioSource.maxDistance = effect.Distance.y;

        audioSource.Play();
        Object.Destroy(soundContainer, effect.Clip.length);
    }
}
