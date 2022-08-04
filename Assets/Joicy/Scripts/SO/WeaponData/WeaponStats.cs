using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    [SerializeField] private ShootingStats _shootingStats;
    [SerializeField] private ProjectileStats _projectileStats;
    [SerializeField] private UpgradeStats _upgradeStats;
    [SerializeField] private ControllableStats _controllableStats;

    public ShootingStats ShootingStats { get => _shootingStats; }
    public ProjectileStats ProjectileStats { get => _projectileStats; }
    public UpgradeStats UpgradeStats { get => _upgradeStats; }
    public ControllableStats ControllableStats { get => _controllableStats; }
}
