using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    [SerializeField] private DisplayStats displayStats;
    [SerializeField] private ShootingStats _shootingStats;
    [SerializeField] private ProjectileStats _projectileStats;
    [SerializeField] private UpgradeStats _upgradeStats;

    public DisplayStats DisplayStats { get => displayStats; }
    public ShootingStats ShootingStats { get => _shootingStats; }
    public ProjectileStats ProjectileStats { get => _projectileStats; }
    public UpgradeStats UpgradeStats { get => _upgradeStats; }
}
