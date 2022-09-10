using System.Collections;
using UnityEngine;
using Zenject;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxSpawnDistance = Vector2.one;
    [SerializeField] private float spawnRadius = 0.5f;
    [SerializeField] private float spawnCooldown = 0.1f;

    [Inject] private DiContainer container = null;
    [Inject] private Level levelSettings = null;
    [Inject] private LevelStats stats = null;

    private Wave[] waves = null;

    private void Awake()
    {
        waves = levelSettings.LevelGameplay.Waves;
        stats.SetMaxWave(waves.Length);
    }

    private void Update()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        if (stats.Wave < stats.MaxWave)
        {
            Wave wave = waves[stats.Wave];

            if (Time.timeSinceLevelLoad > wave.SpawnTime)
            {
                StartCoroutine(SpawnWave(wave));
                stats.IncreaseWave();
            }
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Enemy[] enemies = wave.Enemies;
        Vector2 spawnCenter = GetSpawnCenter();

        for (int enemyTypeIndex = 0; enemyTypeIndex < enemies.Length; enemyTypeIndex++)
        {
            for (int enemyIndex = 0; enemyIndex < wave.EnemiesCount[enemyTypeIndex]; enemyIndex++)
            {
                Vector2 spawnOffset = Utility.FindPointInCircle(spawnRadius);
                Vector3 spawnPoint = new Vector3(spawnCenter.x + spawnOffset.x, 0f, spawnCenter.y + spawnOffset.y);

                SpawnEnemy(enemies[enemyTypeIndex], spawnPoint);

                yield return new WaitForSeconds(spawnCooldown);
            }
        }
    }

    private void SpawnEnemy(Enemy enemy, Vector3 position)
    {
        GameObject enemyObject = container.InstantiatePrefab(enemy.gameObject, position, Quaternion.identity, null);
        enemyObject.transform.LookAt(Vector3.zero);
    }

    private Vector2 GetSpawnCenter()
    {
        float spawnDistance = Random.Range(minMaxSpawnDistance[0], minMaxSpawnDistance[1]);
        return Utility.FindPointOnCircle(spawnDistance);
    }
}
