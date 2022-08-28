using UnityEngine;

[RequireComponent(typeof(Health))]
public class City : MonoBehaviour
{
    [SerializeField] private IntEventChannel cityAttackedChannel = null;
    [SerializeField] private VoidEventChannel gameLostChannel = null;

    private Health health = null;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Died += Death;
        health.DamageTaken += OnDamageTaken;
    }

    private void Death()
    {
        gameLostChannel.RaiseEvent();
    }

    private void OnDamageTaken(int damage)
    {
        cityAttackedChannel.RaiseEvent(damage);
    }

    private void OnDestroy()
    {
        health.Died -= Death;
        health.DamageTaken -= OnDamageTaken;
    }
}
