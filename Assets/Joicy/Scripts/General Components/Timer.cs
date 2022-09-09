using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour, IProjectileDataReceiver
{
    public UnityEvent OnTimerEnd;

    [SerializeField] protected float timer = 1f;
    [SerializeField] private bool reapeatable = false;
    [SerializeField] private float repeatTime = 0f;

    public void SetStats(ProjectileStats projectileStats)
    {
        timer = projectileStats.EngineBurnTime;
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(SetTimer());
    }

    private IEnumerator SetTimer()
    {
        yield return new WaitForSeconds(timer);
        OnTimerEnd.Invoke();

        if(reapeatable)
        {
            StartCoroutine(ResetTimer());
        }
    }

    private IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(repeatTime);
        StartCoroutine(SetTimer());
    }
}
