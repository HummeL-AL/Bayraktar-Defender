using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct ProjectileStats
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private int _penetrationLevel;
    [SerializeField] private Vector2 _minMaxDamage;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _speed;
    [SerializeField] private float _engineBurnTime;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private DecalProjector _landmark;

    public Projectile Projectile { get => _projectile; }
    public int PenetrationLevel { get => _penetrationLevel; }
    public Vector2 MinMaxDamage { get => _minMaxDamage; }
    public float ExplosionRadius { get => _explosionRadius; }
    public float Speed { get => _speed; }
    public float EngineBurnTime { get => _engineBurnTime; }
    public ParticleSystem HitEffect { get => _hitEffect; }
    public AudioClip HitSound { get => _hitSound; }
    public DecalProjector Landmark { get => _landmark; }
}
