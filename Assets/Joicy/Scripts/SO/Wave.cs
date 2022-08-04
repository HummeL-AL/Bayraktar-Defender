using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [SerializeField] private int _spawnTime;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private int[] _enemiesCount;

    public int GetSpawnTime()
    {
        return _spawnTime;
    }

    public Enemy[] GetEnemies()
    {
        return _enemies;
    }

    public int[] GetEnemiesCount()
    {
        return _enemiesCount;
    }
}
