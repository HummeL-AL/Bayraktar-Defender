using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private PlayerWeapon weaponSlot = null;
    [SerializeField] private Weapon weaponPrefab = null;

    [Inject] private DiContainer container = null;
    [Inject] private ResourcesLoader resourcesLoader = null;
    [Inject] private SaveData saveData = null;

    private void Start()
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

                Weapon newWeapon = container.InstantiatePrefab(weaponPrefab, weaponSlot.transform).GetComponent<Weapon>();
                newWeapon.WeaponStats = stats;

                weaponSlot.AddWeapon(newWeapon);
            }
        }
    }

    private void SetDroneData()
    {
        TrySetHealth();
        TrySetSpeed();
        TrySetZoom();
    }

    private bool TrySetHealth()
    {
        Dictionary<string, int> droneUpgrades = saveData.UpgradesData.DroneLevels;

        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health)
        {
            droneUpgrades.TryGetValue("HealthUpgrade", out int upgradeLevel);

            foreach (DroneUpgradeData upgradeData in resourcesLoader.DroneUpgrades)
            {
                HealthUpgradeData healthUpgrades = upgradeData as HealthUpgradeData;
                if (healthUpgrades)
                {
                    HealthUpgrade upgrade = healthUpgrades.GetUpgrade(upgradeLevel) as HealthUpgrade;
                    health.SetStats(upgrade);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TrySetSpeed()
    {
        Dictionary<string, int> droneUpgrades = saveData.UpgradesData.DroneLevels;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement)
        {
            droneUpgrades.TryGetValue("SpeedUpgrade", out int upgradeLevel);

            foreach (DroneUpgradeData upgradeData in resourcesLoader.DroneUpgrades)
            {
                SpeedUpgradeData healthUpgrades = upgradeData as SpeedUpgradeData;
                if (healthUpgrades)
                {
                    SpeedUpgrade upgrade = healthUpgrades.GetUpgrade(upgradeLevel) as SpeedUpgrade;
                    movement.SetStats(upgrade);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TrySetZoom()
    {
        Dictionary<string, int> droneUpgrades = saveData.UpgradesData.DroneLevels;

        CameraViewController cameraController = GetComponent<CameraViewController>();
        if (cameraController)
        {
            droneUpgrades.TryGetValue("ZoomUpgrade", out int upgradeLevel);

            foreach (DroneUpgradeData upgradeData in resourcesLoader.DroneUpgrades)
            {
                ZoomUpgradeData healthUpgrades = upgradeData as ZoomUpgradeData;
                if (healthUpgrades)
                {
                    ZoomUpgrade upgrade = healthUpgrades.GetUpgrade(upgradeLevel) as ZoomUpgrade;
                    cameraController.SetStats(upgrade);
                    return true;
                }
            }
        }

        return false;
    }
}