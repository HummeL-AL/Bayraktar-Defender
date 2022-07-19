using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _reward = 100;

    public virtual void Awake()
    {
        GetComponent<Health>().Died += Death;
        GameController.EnemiesCount++;
        GameEventHandler.EnemySpawned.Invoke(this);
    }

    public virtual void Death()
    {
        GameController.EnemiesCount--;
        GameEventHandler.EnemyDied.Invoke(this);
        Destroy(gameObject);
    }

    public virtual int GetReward()
    {
        return _reward;
    }
}