using System.Collections;
using UnityEngine;
using Zenject;

public class AntiAircraft : MonoBehaviour, IEnemyRole
{
    [SerializeField] private Projectile projectile = null;
    [SerializeField] private WeaponData weaponData = null;
    [SerializeField] private float attackCooldown = 1f;

    [Inject] private Player player = null;

    private Transform playerTransform = null;
    private PlayerMoving movement = null;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(Attack());
    }

    public void Deactivate()
    {
        enabled = false;
    }

    private void Awake()
    {
        playerTransform = player.transform;
        movement = player.GetComponent<PlayerMoving>();
    }

    //Should use better lead calculation method
    private IEnumerator Attack()
    {
        while (true)
        {
            ProjectileStats projectileStats = weaponData.GetWeaponStats(0).ProjectileStats;

            //Vector3 targetPosition = playerTransform.position;
            float lead = 0f;
            //if (movement)
            //{
            //    float distanceToTarget = Vector3.Distance(targetPosition - transform.position, targetPosition);
            //    float estimatedImpactTime = distanceToTarget / projectileStats.Speed;
            //    lead = estimatedImpactTime * movement.Speed;
            //}

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.up, transform.up); //Quaternion.LookRotation(targetPosition - transform.position, transform.up);
            Projectile createdProjectile = Instantiate(projectile, transform.position, targetRotation, null);
            createdProjectile.SetStats(projectileStats);

            SelfGuidance selfGuidance = createdProjectile.GetComponent<SelfGuidance>();
            if (selfGuidance)
            {
                selfGuidance.Target = playerTransform;
                selfGuidance.LeadDistance = lead;
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }
}