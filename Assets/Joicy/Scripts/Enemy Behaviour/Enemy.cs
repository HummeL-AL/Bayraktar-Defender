using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Reward { get => reward; }

    [SerializeField] private EnemyEventChannel spawnChannel = null;
    [SerializeField] private EnemyEventChannel deathChannel = null;

    [SerializeField] private int reward = 100;
    [SerializeField] private float corpseTime = 10f;

    private List<Buff> buffs = new List<Buff>();
    private IEnemyComponent[] enemyComponents = null;
    private EnemyState enemyState = EnemyState.Traveling;

    public void SetState(EnemyState state)
    {
        enemyState = state;
        UpdateState();
    }

    public void AddBuff(string buffType)
    {
        Type buff = Type.GetType(buffType);

        bool buffed = gameObject.TryGetComponent(buff, out Component component);
        if (buffed)
        {
            Buff currentBuff = component as Buff;
            currentBuff.Renovate();
        }
        else
        {
            component = gameObject.AddComponent(buff);
            Buff currentBuff = component as Buff;
            currentBuff.Activate();
            buffs.Add(currentBuff);
        }
    }

    private void Awake()
    {
        spawnChannel.RaiseEvent(this);
        GetEnemyComponents();
        SubscribeToEvents();
    }

    private void Update()
    {
        SetDefaultValues();
        ApplyBuffs();
    }

    private void SetDefaultValues()
    {
        foreach(IEnemyComponent enemyComponent in enemyComponents)
        {
            enemyComponent.SetDefault();
        }
    }

    private void ApplyBuffs()
    {
        foreach (Buff buff in buffs)
        {
            if (buff)
            {
                buff.ApplyEffect();
            }
            else
            {
                buffs.Remove(buff);
            }
        }
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
                    break;
                }
            default:
                {
                    Debug.Log("Uncorrect state type!");
                    break;
                }
        }
    }

    private void SubscribeToEvents()
    {
        Health health = GetComponent<Health>();
        if (health) { health.Died += OnDeath; }

        IEnemyMovement movement = GetComponent<IEnemyMovement>();
        if (movement != null) { movement.TargetDistanceReached += OnTargetReached; }
    }

    private void GetEnemyComponents()
    {
        enemyComponents = GetComponents<IEnemyComponent>();
    }

    private void OnTargetReached()
    {
        SetState(EnemyState.ReachedTarget);
    }

    private void OnDeath()
    {
        SetState(EnemyState.Dead);
        Destroy(gameObject, corpseTime);

        if (UnityEngine.Random.value >= 0.5)
        {
            Vector3 scale = Vector3.one;
            scale.x = -1;
            transform.localScale = scale;
        }

        transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 360f));

        deathChannel.RaiseEvent(this);
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