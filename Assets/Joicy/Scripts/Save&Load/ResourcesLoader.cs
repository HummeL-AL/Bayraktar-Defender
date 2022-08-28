using UnityEngine;

public class ResourcesLoader
{
    public ResourcesLoader()
    {
        LoadData();
    }

    public Campaign[] Campaigns { get; private set; }
    public WeaponData[] Weapons { get; private set; }

    public void LoadData()
    {
        Campaigns = Resources.LoadAll<Campaign>("");
        Weapons = Resources.LoadAll<WeaponData>("");
    }
}
