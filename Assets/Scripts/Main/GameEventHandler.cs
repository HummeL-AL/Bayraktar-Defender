public static class GameEventHandler
{
    public delegate void UIDelegate();
    public static UIDelegate StatsChanged = null;
    public static UIDelegate WeaponSwitched = null;
    public static UIDelegate WeaponStatsChanged = null;

    public delegate void GameDelagate();
    public static GameDelagate WaveChanged = null;

    public delegate void EnemyDelegate(Enemy enemy);
    public static EnemyDelegate EnemySpawned = null;
    public static EnemyDelegate EnemyDied = null;

    public delegate void UpgradeDelegate(IUpgradable upgraded);
    public static UpgradeDelegate WeaponUpgraded = null;
    public static UpgradeDelegate DroneUpgraded = null;
    public static UpgradeDelegate CityUpgraded = null;
}
