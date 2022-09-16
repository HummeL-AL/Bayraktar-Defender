using UnityEngine;
using Zenject;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private WeaponUpgradeButton _prefab = null;

    [Inject] DiContainer container = null;
    [Inject] private ResourcesLoader resourcesLoader = null;

    public void UpdateWeapons()
    {
        Clear();
        DisplayWeapons();
    }

    private void DisplayWeapons()
    {
        for (int i = 0; i < resourcesLoader.Weapons.Length; i++)
        {
            if (resourcesLoader.Weapons[i].IsVisible)
            {
                WeaponUpgradeButton upgrade = container.InstantiatePrefab(_prefab, transform).GetComponent<WeaponUpgradeButton>();
                upgrade.SetWeapon(resourcesLoader.Weapons[i]);
                upgrade.UpdateWeaponInfo();
            }
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
