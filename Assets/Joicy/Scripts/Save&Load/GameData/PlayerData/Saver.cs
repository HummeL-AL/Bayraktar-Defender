using System.Collections.Generic;

public abstract class Saver
{
    protected ResourcesLoader resourcesLoader = null;

    protected Saver(ResourcesLoader newResourcesLoader)
    {
        resourcesLoader = newResourcesLoader;
    }

    public SaveData CreateNewSaveData()
    {
        SaveData data = new SaveData();
        data.Initialize(CreateNewGameData(), CreateNewPlayerData(), CreateNewUpgradesData());
        return data;
    }

    public abstract void CreateSave(SaveData save);
    public abstract SaveData LoadSave();

    private GameData CreateNewGameData()
    {
        int levelsCount = resourcesLoader.Campaigns.Length;
        int[] completedLevels = new int[levelsCount];

        for (int i = 0; i < levelsCount; i++)
        {
            completedLevels[i] = 0;
        }

        return new GameData(completedLevels);
    }
    private PlayerData CreateNewPlayerData()
    {
        return new PlayerData(0);
    }

    private UpgradesData CreateNewUpgradesData()
    {
        return new UpgradesData(CreateWeaponUpgradesData(), CreateDroneUpgradesData());
    }

    private Dictionary<string, int> CreateWeaponUpgradesData()
    {
        Dictionary<string, int> weaponUpgrades = new Dictionary<string, int>();
        foreach (WeaponData weaponUpgrade in resourcesLoader.Weapons)
        {
            int weaponLlevel = weaponUpgrade.UnlockedByDefault ? 1 : 0;
            weaponUpgrades.Add(weaponUpgrade.name, weaponLlevel);
        }

        return weaponUpgrades;
    }

    private Dictionary<string, int> CreateDroneUpgradesData()
    {
        Dictionary<string, int> droneUpgrades = new Dictionary<string, int>();
        foreach (DroneUpgradeData droneUpgrade in resourcesLoader.DroneUpgrades)
        {
            int weaponLlevel = droneUpgrade.UnlockedByDefault ? 1 : 0;
            droneUpgrades.Add(droneUpgrade.Name, weaponLlevel);
        }

        return droneUpgrades;
    }
}