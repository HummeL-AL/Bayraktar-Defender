using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class Explosive : MonoBehaviour, IProjectileDataReceiver
{
    [SerializeField] private float penetrationLevel = 0;
    [SerializeField] private float minDamage = 0;
    [SerializeField] private float maxDamage = 0;
    [SerializeField] private float explosionRadius = 0;
    [SerializeField] private LayerMask affectedLayers = 0;

    [SerializeField] private AudioEffect[] soundEffects = null;
    [SerializeField] private ParticleSystem[] visualEffects = null;
    [SerializeField] private DecalProjector projector = null;

    [Inject] private AudioPlayer audioPlayer = null;
    private float scale = 1f;

    public void SetStats(ProjectileStats projectileStats)
    {
        penetrationLevel = projectileStats.PenetrationLevel;
        minDamage = projectileStats.MinMaxDamage.x;
        maxDamage = projectileStats.MinMaxDamage.y;
        explosionRadius = projectileStats.ExplosionRadius;

        soundEffects = new AudioEffect[1];
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

        foreach (Collider collider in Physics.OverlapSphere(impactPosition, explosionRadius, affectedLayers))
        {
            Transform hittedObject = collider.transform;
            Health hittedHealth = hittedObject.GetComponent<Health>();

            if (penetrationLevel >= hittedHealth.Armor)
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

    private void Awake()
    {
        Health health = GetComponent<Health>();
        if(health)
        {
            health.Died += Explode;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void PlaySounds()
    {
        if (soundEffects != null)
        {
            foreach (AudioEffect effect in soundEffects)
            {
                if (effect != null)
                {
                    audioPlayer.PlaySound(effect, transform.position);
                }
            }
        }
    }

    private void PlayEffects(Vector3 up)
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

    private void SetLandmark(Vector3 normal)
    {
        if (projector != null)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, -normal);

            Transform landmark = Instantiate(projector, transform.position, rotation).transform;
            landmark.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
