using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    public UnityEvent OnFuelBurnout;

    [SerializeField] private GameObject _vfx = null;
    
    private float _minDamage = 0;
    private float _maxDamage = 0;
    private float _explosionRadius = 0;
    private float _speed = 0;
    private float _engineBurnTime = 1f;
    private ParticleSystem _hitEffect = null;
    private AudioClip _hitSound = null;
    private DecalProjector _projector = null;

    private Rigidbody _rigidbody = null;

    public void SetProjectileStats(ProjectileStats projectileStats)
    {
        _minDamage = projectileStats.MinMaxDamage.x;
        _maxDamage = projectileStats.MinMaxDamage.y;
        _explosionRadius = projectileStats.ExplosionRadius;    
        _speed = projectileStats.Speed;
        _engineBurnTime = projectileStats.EngineBurnTime;
        _hitEffect = projectileStats.HitEffect;
        _hitSound = projectileStats.HitSound;
        _projector = projectileStats.Landmark;

        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetControllableStats(Controllable controllable, ControllableStats stats)
    {
        controllable.SetStats(stats);
    }

    private void FixedUpdate()
    {
        Move();
        UpdateFuel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Explode();
        SetLandmark(normal);
    }

    private void UpdateFuel()
    {
        if(_engineBurnTime > 0)
        {
            _engineBurnTime -= Time.fixedDeltaTime;
        }
        else
        {
            OnFuelBurnout.Invoke();
        }
    }

    private void Move()
    {
        _rigidbody.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
    }

    private void Explode()
    {
        float explosionRadius = _explosionRadius;
        float deltaDamage = _maxDamage - _minDamage;

        Vector3 impactPosition = transform.position;

        foreach (Collider collider in Physics.OverlapSphere(impactPosition, explosionRadius, 1 << 3))
        {
            Transform hittedEnemy = collider.transform;
            Health hittedHealth = hittedEnemy.GetComponent<Health>();

            Vector3 closestPoint = Physics.ClosestPoint(impactPosition, collider, hittedEnemy.position, hittedEnemy.rotation);

            float damagePercent = 1 - (Vector3.Distance(closestPoint, transform.position)/explosionRadius);
            int damage = (int)(_minDamage + deltaDamage * damagePercent);

            if (hittedHealth)
            {
                hittedHealth.TakeDamage(damage);
            }
        }

        if (_vfx) _vfx.transform.parent = null;
        PlayEffects();
        Destroy(gameObject);
    }

    private void PlayEffects()
    {
        Vector3 impactPosition = transform.position;

        Instantiate(_hitEffect, impactPosition, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_hitSound, impactPosition);
    }

    private void SetLandmark(Vector3 normal)
    {
        if (_projector)
        {
            Transform landmark = Instantiate(_projector, transform.position, Quaternion.identity).transform;
            landmark.position += normal;
            landmark.LookAt(landmark.position - normal);
            landmark.localScale = new Vector3(_explosionRadius/2f, _explosionRadius/2f, 1f);
        }
    }
}
