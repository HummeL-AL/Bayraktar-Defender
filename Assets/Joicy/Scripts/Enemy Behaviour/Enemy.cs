using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyEventChannel spawnChannel = null;
    [SerializeField] private EnemyEventChannel deathChannel = null;

    [SerializeField] private int reward = 100; 

    public virtual void Awake()
    {
        GetComponent<Health>().Died += Death;

        spawnChannel.RaiseEvent(this);
    }

    public virtual void Death()
    {
        Destroy(gameObject);
        deathChannel.RaiseEvent(this);
    }

    public virtual int GetReward()
    {
        return reward;
    }
}