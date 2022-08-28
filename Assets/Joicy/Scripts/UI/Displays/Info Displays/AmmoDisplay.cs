using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _eventChannel = null;
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        _eventChannel.ChannelEvent += UpdateValue;
    }

    private void UpdateValue()
    {
        Weapon weapon = _playerWeapon.CurrentWeapon;
        display.text = $"Ammo: {weapon.Ammo}/{weapon.ShootingStats.MaxAmmo}";
    }

    private void OnDestroy()
    {
        _eventChannel.ChannelEvent -= UpdateValue;
    }
}