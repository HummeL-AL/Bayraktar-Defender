using UnityEngine;

public class AmmoDisplay : InfoDisplay
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        Weapon weapon = _playerWeapon.CurrentWeapon;
        display.text = $"Ammo: {weapon.Ammo}/{weapon.ShootingStats.MaxAmmo}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}