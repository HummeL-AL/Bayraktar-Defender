using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] WeaponUpgrader upgrader = null;
    [SerializeField] VoidEventChannel weaponStatsChangedChannel = null;

    [SerializeField] Image icon = null;
    [SerializeField] TMP_Text name = null;
    [SerializeField] TMP_Text level = null;
    [SerializeField] TMP_Text description = null;
    [SerializeField] TMP_Text price = null;

    private Weapon _weapon = null;

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
        UpdateWeaponInfo();
    }

    public void UpdateWeaponInfo()
    {
        icon.sprite = _weapon.GetIcon();
        name.text = $"{_weapon.GetName()}";
        description.text = $"{_weapon.GetDescription()}";

        if (_weapon.IsUpgradeAvailable())
        {
            level.text = $"Level: {_weapon.GetCurrentLevel()}";
            price.text = $"${_weapon.GetUpgradePrice()}";
        }
        else
        {
            level.text = $"Max Level";
            price.text = $"Unavailable";
        }
    }

    public void TryToUpgrade()
    {
        upgrader.TryToUpgradeWeapon(_weapon);
        UpdateWeaponInfo();
        weaponStatsChangedChannel.RaiseEvent();
    }
}
