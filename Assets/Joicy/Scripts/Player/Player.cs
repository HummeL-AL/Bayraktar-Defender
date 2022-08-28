using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private VoidEventChannel gameLostChannel = null;

    private Health health = null;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.Died += Death;
    }

    private void Death()
    {
        gameLostChannel.RaiseEvent();
    }

    private void OnDestroy()
    {
        health.Died -= Death;
    }
}
