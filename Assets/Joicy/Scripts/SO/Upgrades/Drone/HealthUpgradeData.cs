using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade_", menuName = "ScriptableObjects/DroneUpgrades/Health", order = 1)]
public class HealthUpgradeData : DroneUpgradeData
{
    [SerializeField] private bool isVisible = true;
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private HealthUpgrade[] upgrades = null;

    public override bool IsVisible { get => isVisible; }
    public override bool UnlockedByDefault { get => unlockedByDefault; }
    public override string Name { get => name; }
    public override int MaxLevel { get => upgrades.Length - 1; }
    public override IDroneUpgrade GetUpgrade(int level)
    {
        return upgrades[level];
    }
}

[System.Serializable]
public class HealthUpgrade : IDroneUpgrade
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [SerializeField] private int maxHealth;
    [SerializeField] private int armorLevel;
    [SerializeField] private int price;

    public string Name { get => upgradeName; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public int MaxHealth { get => maxHealth; }
    public int ArmorLevel { get => armorLevel; }
    public int Price { get => price; }
}