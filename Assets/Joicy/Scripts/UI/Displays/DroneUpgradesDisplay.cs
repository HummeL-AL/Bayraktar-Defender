using UnityEngine;
using Zenject;

public class DroneUpgradesDisplay : MonoBehaviour
{
    [SerializeField] private DroneUpgradeButton _prefab = null;

    [Inject] DiContainer container = null;
    [Inject] private ResourcesLoader resourcesLoader = null;

    public void UpdateUpgrades()
    {
        Clear();
        DisplayUpgrades();
    }

    private void DisplayUpgrades()
    {
        for (int i = 0; i < resourcesLoader.DroneUpgrades.Length; i++)
        {
            DroneUpgradeButton upgrade = container.InstantiatePrefab(_prefab, transform).GetComponent<DroneUpgradeButton>();
            upgrade.SetUpgrade(resourcesLoader.DroneUpgrades[i]);
            upgrade.UpdateUpgradeInfo();
        }
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
