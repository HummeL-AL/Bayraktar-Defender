using UnityEngine;

[CreateAssetMenu(fileName = "NightVisionUpgrade_", menuName = "ScriptableObjects/DroneUpgrades/NightVision", order = 1)]
public class NightVisionUpgradeData : DroneUpgradeData
{
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private NightVisionUpgrade[] upgrades;

    public override string Name { get => name; }
    public override bool UnlockedByDefault { get => unlockedByDefault; }
    public override int MaxLevel { get => upgrades.Length - 1; }
    public override IDroneUpgrade GetUpgrade(int level)
    {
        return upgrades[level];
    }
}

public class NightVisionUpgrade : IDroneUpgrade
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;

    public string Name { get => upgradeName; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public int Price { get => price; }
}