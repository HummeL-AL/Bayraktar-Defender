using UnityEngine;
using Zenject;

public class GameEventHandler : MonoBehaviour
{
    [SerializeField] private VoidEventChannel enemiesCountChannel = null;
    [SerializeField] private EnemyEventChannel enemySpawnedChannel = null;
    [SerializeField] private IntEventChannel enemyAttackChannel = null;
    [SerializeField] private EnemyEventChannel enemyDeathChannel = null;

    [SerializeField] private GameSettings settings = null;
    [Inject] private LevelStats _stats = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        enemySpawnedChannel.ChannelEvent += OnEnemySpawn;
        enemyAttackChannel.ChannelEvent += OnEnemyAttack;
        enemyDeathChannel.ChannelEvent += OnEnemyDeath;
    }

    private void OnEnemySpawn(Enemy enemy)
    {
        _stats.AddEnemies(1);

        enemiesCountChannel.RaiseEvent();
    }

    private void OnEnemyAttack(int damage)
    {
        _stats.AddVictims(damage);
        _stats.AddMoney(damage * -settings.VictimsPenalty);
    }

    private void OnEnemyDeath(Enemy diedEnemy)
    {
        _stats.AddEnemies(-1);
        _stats.AddMoney(diedEnemy.GetReward());

        enemiesCountChannel.RaiseEvent();
    }
}
