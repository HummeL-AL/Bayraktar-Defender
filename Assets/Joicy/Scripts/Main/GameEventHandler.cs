using UnityEngine;
using Zenject;

public class GameEventHandler : MonoBehaviour
{
    [SerializeField] private VoidEventChannel enemiesCountChannel = null;

    [SerializeField] private EnemyEventChannel enemySpawnedChannel = null;
    [SerializeField] private EnemyEventChannel enemyDeathChannel = null;
    [SerializeField] private IntEventChannel cityAttackedChannel = null;
    [SerializeField] private VoidEventChannel gameWonChannel = null;
    [SerializeField] private VoidEventChannel gameLostChannel = null;

    [Inject] private Level levelSettings = null;
    [Inject] private LevelStats levelStats = null;
    [Inject] private SaveData saveData = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        enemySpawnedChannel.ChannelEvent += OnEnemySpawn;
        enemyDeathChannel.ChannelEvent += OnEnemyDeath;
        cityAttackedChannel.ChannelEvent += OnCityAttacked;
        gameWonChannel.ChannelEvent += OnGameWon;
        gameLostChannel.ChannelEvent += OnGameLost;
    }

    private void OnEnemySpawn(Enemy enemy)
    {
        levelStats.AddEnemy(enemy);

        enemiesCountChannel.RaiseEvent();
    }

    private void OnEnemyDeath(Enemy diedEnemy)
    {
        levelStats.RemoveEnemy(diedEnemy);
        levelStats.AddMoney(diedEnemy.Reward);

        enemiesCountChannel.RaiseEvent();
        CheckWinConditions();
    }

    private void OnCityAttacked(int damage)
    {
        levelStats.AddVictims(damage);
        levelStats.AddMoney(damage * -levelSettings.VictimsPenalty);
    }

    private void OnGameWon()
    {
        int currentCampaign = levelSettings.CampaignNumber;
        int currentLevel = levelSettings.LevelNumber;

        int[] completedLevels = saveData.GameData.CompletedLevels;
        if (completedLevels[currentCampaign] == currentLevel)
        {
            completedLevels[currentCampaign] = currentLevel + 1;
        }

        saveData.PlayerData.Money += levelStats.Money;
    }

    private void OnGameLost()
    {

    }

    private void CheckWinConditions()
    {
        if (levelStats.EnemiesCount == 0 && levelStats.Wave == levelStats.MaxWave)
        {
            gameWonChannel.RaiseEvent();
        }
    }

    private void OnDestroy()
    {
        enemySpawnedChannel.ChannelEvent -= OnEnemySpawn;
        enemyDeathChannel.ChannelEvent -= OnEnemyDeath;
        cityAttackedChannel.ChannelEvent -= OnCityAttacked;
        gameWonChannel.ChannelEvent -= OnGameWon;
        gameLostChannel.ChannelEvent -= OnGameLost;
    }
}
