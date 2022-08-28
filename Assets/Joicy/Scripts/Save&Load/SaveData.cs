using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public GameData GameData;
    public PlayerData PlayerData;
    public UpgradesData UpgradesData;

    public void CopyValues(SaveData saveData)
    {
        GameData = saveData.GameData;
        PlayerData = saveData.PlayerData;
        UpgradesData = saveData.UpgradesData;
    }

    public void Initialize(GameData gameData, PlayerData playerData, UpgradesData upgradesData)
    {
        GameData = gameData;
        PlayerData = playerData;
        UpgradesData = upgradesData;
    }
}

[System.Serializable]
public class GameData
{
    [JsonConstructor]
    public GameData(int[] completedLevels)
    {
        CompletedLevels = completedLevels;
    }

    public GameData(List<int> completedLevels)
    {
        CompletedLevels = completedLevels.ToArray();
    }

    public int[] CompletedLevels = null;
}

[System.Serializable]
public class PlayerData
{
    [JsonConstructor]
    public PlayerData(int money)
    {
        Money = money;
    }

    public int Money = 0;
}

[System.Serializable]
public class UpgradesData
{
    [JsonConstructor]
    public UpgradesData(Dictionary<string, int> weaponLevels, Dictionary<string, int> droneLevels)
    {
        WeaponLevels = weaponLevels;
        DroneLevels = droneLevels;
    }

    public Dictionary<string, int> WeaponLevels = null;
    public Dictionary<string, int> DroneLevels = null;
}

public class SettingsData
{

}