using UnityEngine;

[System.Serializable]
public class EnemyGroup
{
    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private int[] _enemiesCount;

    public Enemy[] Enemies { get => _enemies; }
    public int[] EnemiesCount { get => _enemiesCount; }
}
