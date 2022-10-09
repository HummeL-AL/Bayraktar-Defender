using UnityEngine;

public class CurrentWeaponDisplay : InfoDisplay
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        display.text = $"WEP:{_playerWeapon.CurrentWeapon.WeaponStats.DisplayStats.Name}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
