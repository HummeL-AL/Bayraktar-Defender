using UnityEngine;
using Zenject;

public class City : MonoBehaviour, IDamageable
{
    [SerializeField] private IntEventChannel cityAttackedChannel = null;
    [SerializeField] private VoidEventChannel gameLostChannel = null;

    [Inject] private Level levelData = null;

    private int people = 100;

    public void TakeDamage(int damage, int damageLevel)
    {
        if (people > 0)
        {
            people -= damage;
            OnDamageTaken(damage);

            if (people < 0)
            {
                Death();
            }
        }
    }

    public void TakeDamage(int damage, int damageLevel, float explosionRadius)
    {
        TakeDamage((int)explosionRadius, damageLevel);
    }

    private void Awake()
    {
        people = levelData.LevelGameplay.Population;
    }

    private void Death()
    {
        gameLostChannel.RaiseEvent();
    }

    private void OnDamageTaken(int damage)
    {
        cityAttackedChannel.RaiseEvent(damage);
    }
}
