using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerWeapon weaponSlot = null;
    [SerializeField] private Weapon weaponPrefab = null;

    [Inject] private ResourcesLoader resourcesLoader = null;
    [Inject] private SaveData saveData = null;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        SetWeaponData();
        SetDroneData();
        weaponSlot.SetWeapon(0);
    }

    private void SetWeaponData()
    {
        Dictionary<string, int> weapons = saveData.UpgradesData.WeaponLevels;
        foreach (KeyValuePair<string, int> weapon in weapons)
        {
            int weaponLevel = weapon.Value;

            if (weaponLevel > 0)
            {
                WeaponStats stats = new WeaponStats();
                string weaponName = weapon.Key;
                
                foreach(WeaponData data in resourcesLoader.Weapons)
                {
                    if(data.name == weaponName)
                    {
                        stats = data.GetWeaponStats(weaponLevel);
                    }
                }

                Weapon newWeapon = Instantiate(weaponPrefab, weaponSlot.transform);
                newWeapon.WeaponStats = stats;

                weaponSlot.AddWeapon(newWeapon);
            }
        }
    }

    private void SetDroneData()
    {
        Dictionary<string, int> droneUpgrades = saveData.UpgradesData.DroneLevels;
    }
}