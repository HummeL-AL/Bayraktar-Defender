using UnityEngine;

public abstract class DroneUpgradeData : ScriptableObject, IUpgradeData
{
    public abstract bool IsVisible { get; }
    public abstract bool UnlockedByDefault { get; }
    public abstract string Name { get; }
    public abstract int MaxLevel { get; }

    public abstract IDroneUpgrade GetUpgrade(int level);
}

public interface IDroneUpgradeData : IUpgradeData
{
    public string Name { get; }
    public int MaxLevel { get; }

    public IDroneUpgrade GetUpgrade(int level);
}

public interface IDroneUpgrade
{
    string Name { get; }
    string Description { get; }
    Sprite Icon { get; }
    int Price { get; }
}