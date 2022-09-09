public class PlayerHealth : Health
{
    public void SetStats(HealthUpgrade stats)
    {
        _healthPoints = stats.MaxHealth;
        _armorLevel = stats.ArmorLevel;
    }
}