using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LocalSaveManager : ISaver
{
    private string _path = $"{Application.persistentDataPath}/save.json";
    private ResourcesLoader resourcesLoader = null;

    public LocalSaveManager(ResourcesLoader newResourcesLoader)
    {
        resourcesLoader = newResourcesLoader;
    }

    public SaveData CreateNewSave()
    {
        SaveData data = new SaveData();
        data.Initialize(CreateNewGameData(), CreateNewPlayerData(), CreateNewUpgradesData());
        SaveAsJSON(data);
        return data;
    }

    public void CreateSave(SaveData save)
    {
        SaveData data = save;
        SaveAsJSON(data);
    }

    public SaveData LoadSave()
    {
        SaveData save = null;

        if (IsSaveFileExists())
        {
            string json;

            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            json = reader.ReadToEnd();
            reader.Close();

            save = JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            save = CreateNewSave();
        }

        return save;
    }

    private void SaveAsJSON(SaveData save)
    {
        string json = JsonConvert.SerializeObject(save);

        StreamWriter writer = new StreamWriter(_path, false);
        writer.BaseStream.Seek(0, SeekOrigin.Begin);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    private bool IsSaveFileExists()
    {
        if (File.Exists(_path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private GameData CreateNewGameData()
    {
        int levelsCount = resourcesLoader.Campaigns.Length;
        int[] completedLevels = new int[levelsCount];

        for(int i = 0; i < levelsCount; i++)
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
        foreach(WeaponData weaponUpgrade in resourcesLoader.Weapons)
        {
            int weaponLlevel = weaponUpgrade.UnlockedByDefault ? 1 : 0;
            weaponUpgrades.Add(weaponUpgrade.name, weaponLlevel);
        }

        return weaponUpgrades;
    }

    private Dictionary<string, int> CreateDroneUpgradesData()
    {
        Dictionary<string, int> droneUpgrades = new Dictionary<string, int>();
        foreach (IDroneUpgradeData droneUpgrade in resourcesLoader.DroneUpgrades)
        {
            int weaponLlevel = droneUpgrade.UnlockedByDefault ? 1 : 0;
            droneUpgrades.Add(droneUpgrade.Name, weaponLlevel);
        }

        return droneUpgrades;
    }
}
