using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosive : MonoBehaviour, IProjectileComponent
{
    [SerializeField] private float penetrationLevel = 0;
    [SerializeField] private float minDamage = 0;
    [SerializeField] private float maxDamage = 0;
    [SerializeField] private float explosionRadius = 0;

    [SerializeField] private AudioClip[] soundEffects = null;

    [SerializeField] private ParticleSystem[] visualEffects = null;

    [SerializeField] private DecalProjector projector = null;
    private float scale = 1f;

    public void SetStats(ProjectileStats projectileStats)
    {
        penetrationLevel = projectileStats.PenetrationLevel;
        minDamage = projectileStats.MinMaxDamage.x;
        maxDamage = projectileStats.MinMaxDamage.y;
        explosionRadius = projectileStats.ExplosionRadius;

        soundEffects = new AudioClip[1];
        soundEffects[0] = projectileStats.HitSound;

        visualEffects = new ParticleSystem[1];
        visualEffects[0] = projectileStats.HitEffect;

        projector = projectileStats.Landmark;
        scale = projectileStats.ExplosionRadius / 2f;
    }

    public void Explode()
    {
        float deltaDamage = maxDamage - minDamage;

        Vector3 impactPosition = transform.position;

        foreach (Collider collider in Physics.OverlapSphere(impactPosition, explosionRadius, 1 << 3 | 1 << 8))
        {
            Transform hittedObject = collider.transform;
            Health hittedHealth = hittedObject.GetComponent<Health>();

            if (penetrationLevel >= hittedHealth.GetArmor())
            {
                Vector3 closestPoint = Physics.ClosestPoint(impactPosition, collider, hittedObject.position, hittedObject.rotation);

                float damagePercent = 1 - (Vector3.Distance(closestPoint, transform.position) / explosionRadius);
                int damage = (int)(minDamage + deltaDamage * damagePercent);

                if (hittedHealth)
                {
                    hittedHealth.TakeDamage(damage);
                }
            }
        }

        PlaySounds();
        PlayEffects(transform.up);
        SetLandmark(transform.up);
    }

    public void PlaySounds()
    {
        if (soundEffects != null)
        {
            Vector3 playPosition = transform.position;

            foreach (AudioClip clip in soundEffects)
            {
                if (clip != null)
                {
                    AudioSource.PlayClipAtPoint(clip, playPosition);
                }
            }
        }
    }

    public void PlayEffects(Vector3 up)
    {
        if (visualEffects != null)
        {
            Vector3 playPosition = transform.position;

            foreach (ParticleSystem visualEffect in visualEffects)
            {
                if (visualEffect != null)
                {
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.up, up);
                    Instantiate(visualEffect, playPosition, rotation);
                }
            }
        }
    }

    public void SetLandmark(Vector3 normal)
    {
        if (projector != null)
        {
            Transform landmark = Instantiate(projector, transform.position, Quaternion.identity).transform;
            landmark.position += normal;
            landmark.LookAt(landmark.position - normal);
            landmark.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
