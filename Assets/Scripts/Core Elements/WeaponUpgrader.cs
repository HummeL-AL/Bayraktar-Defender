using UnityEngine;

public class WeaponUpgrader : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon = null;

    public void UpgradeCurrentWeapon()
    {
        Weapon choosedWeapon = _playerWeapon.GetCurrentWeapon();
        TryToUpgradeWeapon(choosedWeapon);
    }

    public void UpgradeWeaponWithID(int id)
    {
        Weapon weapon = _playerWeapon.GetWeapon(id);
        TryToUpgradeWeapon(weapon);
    }

    public static bool TryToUpgradeWeapon(Weapon weapon)
    {
        int requiredMoney = weapon.GetUpgradePrice();
        int currentMoney = GameController.GetMoney();

        if (requiredMoney <= currentMoney)
        {
            if (weapon.TryToUpgrade())
            {
                GameController.AddMoney(-requiredMoney);
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
