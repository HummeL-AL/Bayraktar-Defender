using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private LevelStats stats = null;
    [SerializeField] private Vector2 minMaxSpawnDistance = Vector2.one;
    [SerializeField] private float spawnRadius = 0.5f;
    [SerializeField] private float spawnCooldown = 0.1f;
    [SerializeField] private Wave[] waves = null;

    private void Update()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        if (stats.Wave < waves.Length)
        {
            Wave wave = waves[stats.Wave];

            if (Time.timeSinceLevelLoad > wave.GetSpawnTime())
            {
                StartCoroutine(SpawnWave(wave));
                stats.IncreaseWave();
            }
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Enemy[] enemies = wave.GetEnemies();
        Vector2 spawnCenter = GetSpawnCenter();

        for (int enemyTypeIndex = 0; enemyTypeIndex < enemies.Length; enemyTypeIndex++)
        {
            for (int enemyIndex = 0; enemyIndex < wave.GetEnemiesCount()[enemyTypeIndex]; enemyIndex++)
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
        GameObject enemyObject = Instantiate(enemy.gameObject, position, Quaternion.identity);
        enemyObject.transform.LookAt(Vector3.zero);
    }

    private Vector2 GetSpawnCenter()
    {
        float spawnDistance = Random.Range(minMaxSpawnDistance[0], minMaxSpawnDistance[1]);
        return Utility.FindPointOnCircle(spawnDistance);
    }
}
