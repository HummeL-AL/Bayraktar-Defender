using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public List<Enemy> Enemies { get => enemies; private set => enemies = value; }

    public int Money { get; private set; }
    public int Victims { get; private set; }
    public int Wave { get; private set; }
    public int MaxWave { get; private set; }
    public int EnemiesCount { get; private set; }

    [SerializeField] private VoidEventChannel moneyUpdated = null;
    [SerializeField] private VoidEventChannel victimsUpdated = null;
    [SerializeField] private VoidEventChannel waveChanged = null;
    [SerializeField] private VoidEventChannel enemiesChanged = null;

    private List<Enemy> enemies = new List<Enemy>();
    
    public void AddMoney(int increase)
    {
        Money += increase;
        moneyUpdated.RaiseEvent();
    }

    public void AddVictims(int increase)
    {
        Victims += increase;
        victimsUpdated.RaiseEvent();
    }

    public void IncreaseWave()
    {
        Wave++;
        waveChanged.RaiseEvent();
    }

    public void SetMaxWave(int wave)
    {
        MaxWave = wave;
    }

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
        EnemiesCount++;
        enemiesChanged.RaiseEvent();
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        EnemiesCount--;
        enemiesChanged.RaiseEvent();
    }
}
