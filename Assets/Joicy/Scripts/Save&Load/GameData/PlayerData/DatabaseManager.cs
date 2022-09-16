using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
public class DatabaseManager : Saver
{
    DatabaseReference dir = null;
    SaveData loadedSaveData = null;

    public DatabaseManager(ResourcesLoader newResourcesLoader) : base(newResourcesLoader)
    {
        dir = FirebaseDatabase.DefaultInstance.RootReference.Child("Devices").Child(Utility.GetUserID());
    }

    public override void CreateSave(SaveData save)
    {
        dir.Child($"SaveTime").SetValueAsync(save.SaveTime);

        GameData gameData = save.GameData;
        dir.Child($"{gameData}").Child($"CompletedLevels").SetValueAsync(gameData.CompletedLevels);

        PlayerData playerData = save.PlayerData;
        dir.Child($"{playerData}").Child($"Money").SetValueAsync(playerData.Money);

        UpgradesData upgradesData = save.UpgradesData;
        dir.Child($"{upgradesData}").Child($"WeaponLevels").SetValueAsync(upgradesData.WeaponLevels);
        dir.Child($"{upgradesData}").Child($"DroneLevels").SetValueAsync(upgradesData.DroneLevels);

    }

    public override SaveData LoadSave()
    {
        LoadDatabaseSave();
        return loadedSaveData;
    }

    private async void LoadDatabaseSave()
    {
        SaveData saveData = CreateNewSaveData();

        Task<DataSnapshot> allData = dir.GetValueAsync();
        DataSnapshot databaseSnapshot = await allData;

        saveData.SaveTime = (string)databaseSnapshot.Child($"SaveTime").Value;

        GameData gameData = saveData.GameData;
        List<int> completedLevels = new List<int>();
        foreach (DataSnapshot snapshot in databaseSnapshot.Child($"{gameData}").Child($"CompletedLevels").Children)
        {
            completedLevels.Add(Convert.ToInt32(snapshot.Value));
        }
        gameData.CompletedLevels = completedLevels.ToArray();

        PlayerData playerData = saveData.PlayerData;
        playerData.Money = Convert.ToInt32(databaseSnapshot.Child($"{playerData}").Child($"Money").Value);

        UpgradesData upgradesData = saveData.UpgradesData;
        Dictionary<string, int> weaponUpgrades = new Dictionary<string, int>();
        foreach (DataSnapshot snapshot in databaseSnapshot.Child($"{upgradesData}").Child($"WeaponLevels").Children)
        {
            string upgradeName = snapshot.Key;
            int upgradeLevel = Convert.ToInt32(snapshot.Value);
            weaponUpgrades.Add(upgradeName, upgradeLevel);
        }
        upgradesData.WeaponLevels = weaponUpgrades;

        Dictionary<string, int> droneUpgrades = new Dictionary<string, int>();
        foreach (DataSnapshot snapshot in databaseSnapshot.Child($"{upgradesData}").Child($"WeaponLevels").Children)
        {
            string upgradeName = snapshot.Key;
            int upgradeLevel = Convert.ToInt32(snapshot.Value);
            droneUpgrades.Add(upgradeName, upgradeLevel);
        }
        upgradesData.DroneLevels = droneUpgrades;

        loadedSaveData = saveData;
    }
}
