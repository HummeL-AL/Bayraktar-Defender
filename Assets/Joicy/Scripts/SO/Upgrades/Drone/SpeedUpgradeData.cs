using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpgrade_", menuName = "ScriptableObjects/DroneUpgrades/Speed", order = 1)]
public class SpeedUpgradeData : DroneUpgradeData
{
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private SpeedUpgrade[] upgrades;

    public override string Name { get => name; }
    public override bool UnlockedByDefault { get => unlockedByDefault; }
    public override int MaxLevel { get => upgrades.Length - 1; }
    public override IDroneUpgrade GetUpgrade(int level)
    {
        return upgrades[level];
    }
}

[System.Serializable]
public class SpeedUpgrade : IDroneUpgrade
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private int price;

    public string Name { get => upgradeName; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public float MinSpeed { get => minSpeed; }
    public float MaxSpeed { get => maxSpeed; }
    public float Acceleration { get => acceleration; }
    public int Price { get => price; }
}