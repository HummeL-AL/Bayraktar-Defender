using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static bool GamePaused { get; set; }
    public static int EnemiesCount = 0;

    [SerializeField] private PlayerInput[] _inputs = null;
    [SerializeField] private PlayerInput _playerInput = null;

    [SerializeField] private Vector2 _minMaxSpawnDistance = Vector2.one;
    [SerializeField] private float _spawnRadius = 0.5f;
    [SerializeField] private float _spawnCooldown = 0.1f;
    [SerializeField] private Wave[] _waves = null;

    [SerializeField] private static int _money = 0;
    [SerializeField] private static int _victims = 0;
    [SerializeField] private static int _victimPenalty = 300;

    private static int _waveCount = 0;

    public static int GetMoney()
    {
        return _money;
    }

    public static void AddMoney(int income)
    {
        _money += income;
    }

    public static int GetVictims()
    {
        return _victims;
    }

    public static int GetWave()
    {
        return _waveCount;
    }

    public void Pause()
    {
        _playerInput.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GamePaused = true;
    }

    public void Resume()
    {
        _playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GamePaused = false;
    }

    private void Awake()
    {
        Utility.Controller = this;
        GameEventHandler.EnemySpawned += OnEnemySpawn;
        GameEventHandler.EnemyDied += OnEnemyDeath;
        EnableInputs();
    }

    private void Update()
    {
        UpdateWave();
    }

    private void UpdateWave()
    {
        if (_waveCount < _waves.Length)
        {
            Wave wave = _waves[_waveCount];

            if (Time.timeSinceLevelLoad > wave.GetSpawnTime())
            {
                StartCoroutine(SpawnWave(wave));
                _waveCount++;
                GameEventHandler.WaveChanged.Invoke();
            }
        }
    }

    private void EnableInputs()
    {
        foreach(PlayerInput input in _inputs)
        {
            input.enabled = true;
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Enemy[] enemies = wave.GetEnemies();
        Vector2 spawnCenter = GetSpawnCenter();

        for (int enemyTypeIndex = 0; enemyTypeIndex < enemies.Length; enemyTypeIndex++)
        {
            for(int enemyIndex = 0; enemyIndex < wave.GetEnemiesCount()[enemyTypeIndex]; enemyIndex++)
            {
                Vector2 spawnOffset = Utility.FindPointInCircle(_spawnRadius);
                Vector3 spawnPoint = new Vector3(spawnCenter.x + spawnOffset.x, 0f, spawnCenter.y + spawnOffset.y) ;

                SpawnEnemy(enemies[enemyTypeIndex], spawnPoint);

                yield return new WaitForSeconds(_spawnCooldown);
            }
        }
    }

    private Vector2 GetSpawnCenter()
    {
        float spawnDistance = Random.Range(_minMaxSpawnDistance[0], _minMaxSpawnDistance[1]);
        return Utility.FindPointOnCircle(spawnDistance);
    }

    private void SpawnEnemy(Enemy enemy, Vector3 position)
    {
        GameObject enemyObject = Instantiate(enemy.gameObject, position, Quaternion.identity);
        enemyObject.transform.LookAt(Vector3.zero);
    }

    private void OnEnemySpawn(Enemy enemy)
    {
        Attacker attacker = enemy.GetComponent<Attacker>();

        if (attacker)
        {
            attacker.Attacking += OnEnemyAttack;
        }
    }

    private void OnEnemyAttack(int damage)
    {
        _victims += damage;
        _money -= damage * _victimPenalty;

        GameEventHandler.StatsChanged.Invoke();
    }

    private void OnEnemyDeath(Enemy diedEnemy)
    {
        _money += diedEnemy.GetReward();

        GameEventHandler.StatsChanged.Invoke();
    }
}
