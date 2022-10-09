using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private int _spawnTime;
    [SerializeField] private EnemyGroup[] _enemyGroups;
    [SerializeField] private int[] _groupCounts;

    public int SpawnTime { get => _spawnTime; }
    public EnemyGroup[] EnemyGroups { get => _enemyGroups; }
    public int[] GroupCounts { get => _groupCounts; }
}
