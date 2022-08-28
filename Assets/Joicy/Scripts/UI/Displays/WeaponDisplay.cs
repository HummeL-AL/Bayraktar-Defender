using UnityEngine;
using Zenject;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private WeaponUpgrade _prefab = null;

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
            WeaponUpgrade upgrade = container.InstantiatePrefab(_prefab, transform).GetComponent<WeaponUpgrade>();
            upgrade.SetWeapon(resourcesLoader.Weapons[i]);
            upgrade.UpdateWeaponInfo();
        }
    }

    private void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
