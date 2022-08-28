using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public UnityEvent OnDeath;

    [SerializeField] private float selfDestroyTime = 10f;

    public void SetStats(ProjectileStats projectileStats)
    {
        selfDestroyTime = projectileStats.SelfDestroyTime;

        foreach (IProjectileComponent projectileComponent in GetComponents<IProjectileComponent>())
        {
            projectileComponent.SetStats(projectileStats);
        }
    }

    private void Awake()
    {
        StartCoroutine(SelfDestroy());
        FindDeathConditions();
    }

    private void FindDeathConditions()
    {
        ProjectileMovement movement = GetComponent<ProjectileMovement>();
        if (movement)
        {
            movement.ObstacleDetected += Death;
        }
    }

    private void Death()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(selfDestroyTime);
        Death();
    }
}
