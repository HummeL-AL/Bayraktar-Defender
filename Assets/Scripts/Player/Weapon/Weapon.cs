using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour, IUpgradable
{
    [Header("References")]
    [SerializeField] private WeaponData _weaponData = null;

    [Header("Info")]
    [SerializeField] private string _name = null;
    [SerializeField] private string _description = null;
    [SerializeField] private Sprite _icon = null;

    [Header("Weapon state")]
    [SerializeField] private int _weaponLevel = 0;
    [SerializeField] private int _ammo = 20;
    [SerializeField] private float _heat = 0;
    [SerializeField] private bool _reloaded = true;
    [SerializeField] private bool _overheated = false;

    private ShootingStats _shootingStats;
    private float _coolingSpeed = 0f;

    public bool IsAbleToShoot()
    {
        if (_reloaded && !_overheated)
        {
            return true;
        }
        else { return false; }
    }

    public bool IsUpgradeAvailable()
    {
        if(_weaponLevel >= _weaponData.MaxLevel)
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
        return _name;
    }

    public string GetDescription()
    {
        return _description;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }

    public int GetUpgradePrice()
    {
        return GetWeaponData().GetWeaponStats(_weaponLevel).UpgradeStats.Price;
    }

    public WeaponData GetWeaponData()
    {
        return _weaponData;
    }

    public int GetWeaponLevel()
    {
        return _weaponLevel;
    }

    public int GetCurrentLevel()
    {
        return _weaponLevel;
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
        _weaponLevel++;
        UpdateStats();
    }

    private void UpdateStats()
    {
        _shootingStats = _weaponData.GetWeaponStats(_weaponLevel).ShootingStats;
        _ammo = _shootingStats.MaxAmmo;
        _coolingSpeed = _shootingStats.CoolingSpeed;
    }

    private void UpdateHeat()
    {
        if (_heat <= 0)
        {
            _overheated = false;
        }
        else
        {
            if (_heat >= 1)
            {
                _overheated = true;
            }

            _heat -= _coolingSpeed * Time.deltaTime;
        }
    }

    private IEnumerator ShootRoutine(Vector3 targetPosition)
    {
        if (!IsNeedToReload())
        {
            Vector3 shootPosition = transform.position;
            WeaponStats weaponStats = _weaponData.GetWeaponStats(_weaponLevel);
            ProjectileStats projectileStats = weaponStats.ProjectileStats;
            ControllableStats controllableStats = weaponStats.ControllableStats;

            GameObject projectileObject = Instantiate(projectileStats.Projectile.gameObject, shootPosition, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            Controllable controllable = projectile.GetComponent<Controllable>();

            projectileObject.transform.LookAt(targetPosition);
            projectile.SetProjectileStats(projectileStats);
            if (controllable) projectile.SetControllableStats(controllable, controllableStats);

            float radialOffset = Random.Range(0f, 360f);
            float spreadPercent = Random.Range(0f, 1f);
            Vector3 rotationVector = Quaternion.AngleAxis(radialOffset, Vector3.forward) * Vector3.right;
            projectile.transform.rotation *= Quaternion.AngleAxis(_shootingStats.SpreadAngle * spreadPercent, rotationVector);

            //projectile.transform.Rotate(Vector3.forward * radialOffset);
            //projectile.transform.Rotate(Vector3.right * _shootingStats.SpreadAngle * spreadPercent - Vector3.forward * radialOffset);

            AudioSource.PlayClipAtPoint(_shootingStats.ShootSound, shootPosition);

            _heat += _shootingStats.ShotHeat;
            _ammo--;
            _reloaded = false;
            yield return new WaitForSeconds(_shootingStats.ShotDelay);
            _reloaded = true;
        }
        else
        {
            StartCoroutine(Reload());
        }
    }

    private bool IsNeedToReload()
    {
        if(_ammo <= 0)
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
        _reloaded = false;
        yield return new WaitForSeconds(_shootingStats.ReloadTime);
        _ammo = _shootingStats.MaxAmmo;
        _reloaded = true;
    }
}
