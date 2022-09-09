using UnityEngine;

[CreateAssetMenu(fileName = "ZoomUpgrade_", menuName = "ScriptableObjects/DroneUpgrades/Zoom", order = 1)]
public class ZoomUpgradeData : DroneUpgradeData
{
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private ZoomUpgrade[] upgrades;

    public override string Name { get => name; }
    public override bool UnlockedByDefault { get => unlockedByDefault; }
    public override int MaxLevel { get => upgrades.Length - 1; }
    public override IDroneUpgrade GetUpgrade(int level)
    {
        return upgrades[level];
    }
}

[System.Serializable]
public class ZoomUpgrade : IDroneUpgrade
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private int price;

    public string Name { get => upgradeName; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public float MinZoom { get => minZoom; }
    public float MaxZoom { get => maxZoom; }
    public int Price { get => price; }
}