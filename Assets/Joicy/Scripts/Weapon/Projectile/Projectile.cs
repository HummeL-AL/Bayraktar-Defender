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

        foreach (IProjectileDataReceiver projectileComponent in GetComponents<IProjectileDataReceiver>())
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
            movement.ObstacleDetected += OnObstacleHit;
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

    private void OnObstacleHit(RaycastHit hit)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        Death();
    }
}
