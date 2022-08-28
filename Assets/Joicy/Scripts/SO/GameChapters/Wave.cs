using UnityEngine;

[CreateAssetMenu(fileName = "Wave_", menuName = "Chapters/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [SerializeField] private int _spawnTime;
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private int[] _enemiesCount;

    public int SpawnTime { get => _spawnTime; }
    public Enemy[] Enemies { get => _enemies; }
    public int[] EnemiesCount { get => _enemiesCount; }
}
