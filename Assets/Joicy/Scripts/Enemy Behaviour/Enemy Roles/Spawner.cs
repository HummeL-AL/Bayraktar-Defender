using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour, IEnemyRole
{
    [SerializeField] private Enemy _spawnableEnemy = null;
    [SerializeField] private float _groupSpawnCooldown = 2f;
    [SerializeField] private float _spawnCooldown = 0.25f;
    [SerializeField] private int _groupCount = 2;
    [SerializeField] private int _groupSize = 5;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(SpawnEnemyGroup());
    }

    public void Deactivate()
    {
        enabled = false;
    }

    private void SpawnEnemy(Enemy enemy)
    {
        GameObject enemyObject = Instantiate(enemy.gameObject, transform.position - 3 * transform.forward, Quaternion.identity);
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
