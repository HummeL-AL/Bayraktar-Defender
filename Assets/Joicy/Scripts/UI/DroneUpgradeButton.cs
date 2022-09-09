using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class DroneUpgradeButton : MonoBehaviour
{
    [SerializeField] private VoidEventChannel moneyChangedChannel = null;

    [SerializeField] private Image icon = null;
    [SerializeField] private TMP_Text upgradeName = null;
    [SerializeField] private TMP_Text level = null;
    [SerializeField] private TMP_Text description = null;
    [SerializeField] private TMP_Text price = null;

    [Inject] private SaveData saveData = null;

    private int upgradeLevel = 0;
    private DroneUpgradeData upgradeData = null;

    public void SetUpgrade(DroneUpgradeData newData)
    {
        upgradeData = newData;
        UpdateUpgradeInfo();
    }

    public void TryToUpgrade()
    {
        Dictionary<string, int> upgrades = saveData.UpgradesData.DroneLevels;
        IDroneUpgrade upgrade = upgradeData.GetUpgrade(upgradeLevel);

        int currentMoney = saveData.PlayerData.Money;
        int neededMoney = upgrade.Price;
        if (currentMoney >= neededMoney)
        {
            upgrades.Remove(upgradeData.name);
            saveData.PlayerData.Money -= neededMoney;
            upgrades.Add(upgradeData.name, upgradeLevel + 1);

            moneyChangedChannel.RaiseEvent();
        }

        UpdateUpgradeInfo();
    }

    public void UpdateUpgradeInfo()
    {
        UpdateLevel();

        IDroneUpgrade upgrade = upgradeData.GetUpgrade(upgradeLevel);
        icon.sprite = upgrade.Icon;
        upgradeName.text = upgrade.Name;
        description.text = upgrade.Description;

        if (upgradeLevel < upgradeData.MaxLevel)
        {
            level.text = $"Level: {upgradeLevel}";
            price.text = $"${upgrade.Price}";
        }
        else
        {
            level.text = $"Max Level";
            price.text = $"Unavailable";
        }
    }

    private void UpdateLevel()
    {
        upgradeLevel = saveData.UpgradesData.DroneLevels[upgradeData.name];
    }
}
