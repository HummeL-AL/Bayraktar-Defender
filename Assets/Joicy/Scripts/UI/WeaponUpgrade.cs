using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class WeaponUpgrade : MonoBehaviour
{
    [SerializeField] private VoidEventChannel moneyChangedChannel = null;

    [SerializeField] private Image icon = null;
    [SerializeField] private TMP_Text weaponName = null;
    [SerializeField] private TMP_Text level = null;
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private TMP_Text price = null;

    [Inject] private SaveData saveData = null;

    private int weaponLevel = 0;
    private WeaponData weapon = null;

    public void SetWeapon(WeaponData weaponData)
    {
        weapon = weaponData;
        UpdateWeaponInfo();
    }

    public void TryToUpgrade()
    {
        Dictionary<string, int> weapons = saveData.UpgradesData.WeaponLevels;

        int currentMoney = saveData.PlayerData.Money;
        int neededMoney = weapon.GetWeaponStats(weaponLevel).UpgradeStats.Price;
        if (currentMoney >= neededMoney)
        {
            weapons.Remove(weapon.name);
            saveData.PlayerData.Money -= neededMoney;
            weapons.Add(weapon.name, weaponLevel + 1);

            moneyChangedChannel.RaiseEvent();
        }

        UpdateWeaponInfo();
    }

    public void UpdateWeaponInfo()
    {
        UpdateLevel();

        DisplayStats displayStats = weapon.GetWeaponStats(weaponLevel).DisplayStats;
        icon.sprite = displayStats.Icon;
        weaponName.text = $"{displayStats.Name}";
        description.text = $"{displayStats.Description}";

        if (weaponLevel < weapon.MaxLevel)
        {
            level.text = $"Level: {weaponLevel}";
            price.text = $"${weapon.GetWeaponStats(weaponLevel).UpgradeStats.Price}";
        }
        else
        {
            level.text = $"Max Level";
            price.text = $"Unavailable";
        }
    }

    private void UpdateLevel()
    {
        weaponLevel = saveData.UpgradesData.WeaponLevels[weapon.name];
    }
}
