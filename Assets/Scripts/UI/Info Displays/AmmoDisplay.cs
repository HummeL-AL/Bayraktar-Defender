using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        GameEventHandler.WeaponStatsChanged += UpdateValue;
    }

    private void UpdateValue()
    {
        Weapon weapon = _playerWeapon.GetCurrentWeapon();
        display.text = $"Ammo: {weapon.GetAmmo()}/{weapon.GetShootingStats().MaxAmmo}";
    }
}