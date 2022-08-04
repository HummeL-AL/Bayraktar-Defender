using UnityEngine;
using Zenject;

public class WeaponUpgrader : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerWeapon = null;

    [Inject] private LevelStats _stats = null;

    public void UpgradeCurrentWeapon()
    {
        Weapon choosedWeapon = playerWeapon.GetCurrentWeapon();
        TryToUpgradeWeapon(choosedWeapon);
    }

    public void UpgradeWeaponWithID(int id)
    {
        Weapon weapon = playerWeapon.GetWeapon(id);
        TryToUpgradeWeapon(weapon);
    }

    public bool TryToUpgradeWeapon(Weapon weapon)
    {
        int requiredMoney = weapon.GetUpgradePrice();
        int currentMoney = _stats.Money;

        if (requiredMoney <= currentMoney)
        {
            if (weapon.TryToUpgrade())
            {
                _stats.AddMoney(-requiredMoney);
            }
            Debug.Log($"Weapon upgraded successfull! You paid: {requiredMoney}!");
            return true;
        }
        else
        {
            Debug.Log($"Upgrade failed! You have {currentMoney}, but upgrade requires {requiredMoney}.");
            return false;
        }
    }
}
