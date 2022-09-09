using System.Collections;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour, IActivableRole
{
    [SerializeField] private Enemy _spawnableEnemy = null;
    [SerializeField] private float _groupSpawnCooldown = 2f;
    [SerializeField] private float _spawnCooldown = 0.25f;
    [SerializeField] private int _groupCount = 2;
    [SerializeField] private int _groupSize = 5;

    [Inject] private DiContainer container = null;

    private IEnumerator spawn = null;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(spawn);
    }

    public void Deactivate()
    {
        enabled = false;
        StopCoroutine(spawn);
    }

    public void SetDefault()
    {
    }

    private void Awake()
    {
        spawn = SpawnEnemyGroup();
    }

    private void SpawnEnemy(Enemy enemy)
    {
        GameObject enemyObject = container.InstantiatePrefab(enemy.gameObject, transform.position - 3 * transform.forward, Quaternion.identity, null);
        enemyObject.transform.LookAt(Vector3.zero);
    }

    private IEnumerator SpawnEnemyGroup()
    {
        for(int i = 0; i < _groupCount; i++)
        {
            for(int j = 0; j < _groupSize; j++)
            {
                SpawnEnemy(_spawnableEnemy);
                yield return new WaitForSeconds(_spawnCooldown);
            }
            yield return new WaitForSeconds(_groupSpawnCooldown);
        }
    }
}
