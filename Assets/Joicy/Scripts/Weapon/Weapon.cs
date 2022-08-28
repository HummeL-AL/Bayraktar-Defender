using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponStats weaponStats;
    [SerializeField] private VoidEventChannel weaponStatsChanged = null;

    [Header("Weapon state")]
    [SerializeField] private int weaponLevel = 0;
    [SerializeField] private int ammo = 20;
    [SerializeField] private float heat = 0;
    [SerializeField] private bool reloaded = true;
    [SerializeField] private bool overheated = false;

    private ShootingStats _shootingStats;
    private float _coolingSpeed = 0f;

    public WeaponStats WeaponStats
    {
        get => weaponStats;
        set
        {
            weaponStats = value;
            UpdateStats();
        }
    }

    public int UpgradePrice { get => weaponStats.UpgradeStats.Price; }
    public ShootingStats ShootingStats { get => _shootingStats; }
    public int CurrentLevel { get => weaponLevel; }
    public float Ammo { get => ammo; }
    public float Heat { get => heat; }

    public bool IsAbleToShoot { get => reloaded && !overheated; }

    public void Shoot(Vector3 targetPosition)
    {
        StartCoroutine(ShootRoutine(targetPosition));
    }

    private void Awake()
    {
        UpdateStats();
    }

    private void Update()
    {
        UpdateHeat();
    }

    private void UpdateStats()
    {
        _shootingStats = WeaponStats.ShootingStats;
        ammo = _shootingStats.MaxAmmo;
        _coolingSpeed = _shootingStats.CoolingSpeed;
    }

    private void UpdateHeat()
    {
        if (heat <= 0)
        {
            overheated = false;
        }
        else
        {
            if (heat >= 1 && _shootingStats.Overheatable)
            {
                overheated = true;
            }

            heat -= _coolingSpeed * Time.deltaTime;
        }
        weaponStatsChanged.RaiseEvent();
    }

    private IEnumerator ShootRoutine(Vector3 targetPosition)
    {
        if (!IsNeedToReload())
        {
            Vector3 shootPosition = transform.position;
            ProjectileStats projectileStats = weaponStats.ProjectileStats;

            GameObject projectileObject = Instantiate(projectileStats.Projectile.gameObject, shootPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectileObject.transform.LookAt(targetPosition);
            projectile.SetStats(projectileStats);

            float spreadPercent = Random.Range(0f, 1f);
            float radialOffset = Random.Range(0f, 360f);

            Vector2 minMaxSpread = _shootingStats.SpreadAngle;
            float spreadAngle = minMaxSpread.x + (minMaxSpread.y - minMaxSpread.x) * heat;

            Vector3 rotationVector = Quaternion.AngleAxis(radialOffset, Vector3.forward) * Vector3.right;
            projectile.transform.rotation *= Quaternion.AngleAxis(spreadAngle * spreadPercent, rotationVector);

            AudioSource.PlayClipAtPoint(_shootingStats.ShootSound, shootPosition);

            heat += _shootingStats.ShotHeat;
            ammo--;
            reloaded = false;
            weaponStatsChanged.RaiseEvent();
            yield return new WaitForSeconds(_shootingStats.ShotDelay);
            reloaded = true;
        }
        else
        {
            StartCoroutine(Reload());
        }
    }

    private bool IsNeedToReload()
    {
        if(ammo <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator Reload()
    {
        reloaded = false;
        weaponStatsChanged.RaiseEvent();
        yield return new WaitForSeconds(_shootingStats.ReloadTime);
        ammo = _shootingStats.MaxAmmo;
        reloaded = true;
        weaponStatsChanged.RaiseEvent();
    }
}
