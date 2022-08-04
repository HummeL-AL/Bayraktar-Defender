using UnityEngine;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;
    [SerializeField] private WeaponUpgrade _prefab = null;

    public void UpdateWeapons()
    {
        Clear();
        DisplayWeapons();
    }

    private void DisplayWeapons()
    {
        for(int i = 0; i < _playerWeapon.GetWeaponCount(); i++)
        {
            WeaponUpgrade upgrade = Instantiate(_prefab, transform);
            upgrade.SetWeapon(_playerWeapon.GetWeapon(i));
            upgrade.UpdateWeaponInfo();
        }
    }

    private void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
