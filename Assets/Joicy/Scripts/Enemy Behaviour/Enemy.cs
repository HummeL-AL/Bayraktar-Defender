using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Reward { get => reward; }

    [SerializeField] private EnemyEventChannel spawnChannel = null;
    [SerializeField] private EnemyEventChannel deathChannel = null;

    [SerializeField] private int reward = 100;
    [SerializeField] private float corpseTime = 10f;
    [SerializeField] private GameObject[] models = new GameObject[2];

    private EnemyState enemyState = EnemyState.Traveling;

    public void SetState(EnemyState state)
    {
        enemyState = state;
        UpdateState();
    }

    private void Awake()
    {
        spawnChannel.RaiseEvent(this);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        Health health = GetComponent<Health>();
        if (health) { health.Died += OnDeath; }

        IEnemyMovement movement = GetComponent<IEnemyMovement>();
        if (movement != null) { movement.TargetDistanceReached += OnTargetReached; }
    }

    private void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.Traveling:
                {
                    break;
                }
            case EnemyState.ReachedTarget:
                {
                    ToggleMovement(false);
                    ToggleRoles(true);
                    break;
                }
            case EnemyState.Dead:
                {
                    ToggleAllComponents(false);
                    SwitchModel();
                    break;
                }
            default:
                {
                    Debug.Log("Uncorrect state type!");
                    break;
                }
        }
    }

    private void OnTargetReached()
    {
        SetState(EnemyState.ReachedTarget);
    }

    private void OnDeath()
    {
        SetState(EnemyState.Dead);
        Destroy(gameObject, corpseTime);
        deathChannel.RaiseEvent(this);
    }

    private void SwitchModel()
    {
        models[0].SetActive(!models[0].activeSelf);
        models[1].SetActive(!models[1].activeSelf);
    }

    private void ToggleMovement(bool toggleMode)
    {
        ToggleComponents(toggleMode, GetComponents<IEnemyMovement>());
    }

    private void ToggleRoles(bool toggleMode)
    {
        ToggleComponents(toggleMode, GetComponents<IEnemyRole>());
    }

    private void ToggleAllComponents(bool toggleMode)
    {
        ToggleComponents(toggleMode, GetComponents<IEnemyComponent>());
    }

    private void ToggleComponents(bool toggleMode, IEnemyComponent[] enemyComponents)
    {
        foreach (IEnemyComponent component in enemyComponents)
        {
            if (toggleMode)
            {
                component.Activate();
            }
            else
            {
                component.Deactivate();
            }
        }
    }
}