using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IUpgradable
{
    [Header("References")]
    [SerializeField] private WeaponData weaponData = null;
    [SerializeField] private VoidEventChannel weaponStatsChanged = null;

    [Header("Info")]
    [SerializeField] private string name = null;
    [SerializeField] private string description = null;
    [SerializeField] private Sprite icon = null;

    [Header("Weapon state")]
    [SerializeField] private int weaponLevel = 0;
    [SerializeField] private int ammo = 20;
    [SerializeField] private float heat = 0;
    [SerializeField] private bool reloaded = true;
    [SerializeField] private bool overheated = false;

    private ShootingStats _shootingStats;
    private float _coolingSpeed = 0f;

    public bool IsAbleToShoot()
    {
        if (reloaded && !overheated)
        {
            return true;
        }
        else { return false; }
    }

    public bool IsUpgradeAvailable()
    {
        if(weaponLevel >= weaponData.MaxLevel)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Shoot(Vector3 targetPosition)
    {
        StartCoroutine(ShootRoutine(targetPosition));
    }

    public bool TryToUpgrade()
    {
        if (IsUpgradeAvailable())
        {
            Upgrade();
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetUpgradePrice()
    {
        return GetWeaponData().GetWeaponStats(weaponLevel).UpgradeStats.Price;
    }

    public WeaponData GetWeaponData()
    {
        return weaponData;
    }

    public ShootingStats GetShootingStats()
    {
        return _shootingStats;
    }

    public int GetCurrentLevel()
    {
        return weaponLevel;
    }
    public float GetAmmo()
    {
        return ammo;
    }

    public float GetHeat()
    {
        return heat;
    }

    private void Awake()
    {
        UpdateStats();
    }

    private void Update()
    {
        UpdateHeat();
    }

    private void Upgrade()
    {
        weaponLevel++;
        UpdateStats();
    }

    private void UpdateStats()
    {
        _shootingStats = weaponData.GetWeaponStats(weaponLevel).ShootingStats;
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
            WeaponStats weaponStats = weaponData.GetWeaponStats(weaponLevel);
            ProjectileStats projectileStats = weaponStats.ProjectileStats;
            ControllableStats controllableStats = weaponStats.ControllableStats;

            GameObject projectileObject = Instantiate(projectileStats.Projectile.gameObject, shootPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            Controllable controllable = projectile.GetComponent<Controllable>();

            projectileObject.transform.LookAt(targetPosition);
            projectile.SetProjectileStats(projectileStats);
            if (controllable) projectile.SetControllableStats(controllable, controllableStats);

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
