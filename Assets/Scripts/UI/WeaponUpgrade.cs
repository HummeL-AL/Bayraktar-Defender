using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] Image _icon = null;
    [SerializeField] TMP_Text _name = null;
    [SerializeField] TMP_Text _level = null;
    [SerializeField] TMP_Text _description = null;
    [SerializeField] TMP_Text _price = null;

    private Weapon _weapon = null;

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
        UpdateWeaponInfo();
    }

    public void UpdateWeaponInfo()
    {
        _icon.sprite = _weapon.GetIcon();
        _name.text = $"{_weapon.GetName()}";
        _description.text = $"{_weapon.GetDescription()}";

        if (_weapon.IsUpgradeAvailable())
        {
            _level.text = $"Level: {_weapon.GetCurrentLevel()}";
            _price.text = $"${_weapon.GetUpgradePrice()}";
        }
        else
        {
            _level.text = $"Max Level";
            _price.text = $"Unavailable";
        }
    }

    public void TryToUpgrade()
    {
        WeaponUpgrader.TryToUpgradeWeapon(_weapon);
        UpdateWeaponInfo();
        GameEventHandler.StatsChanged.Invoke();
    }
}
