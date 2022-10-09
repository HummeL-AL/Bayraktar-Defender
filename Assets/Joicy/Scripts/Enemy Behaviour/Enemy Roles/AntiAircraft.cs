using System.Collections;
using UnityEngine;
using Zenject;

public class AntiAircraft : MonoBehaviour, IActivableRole
{
    [SerializeField] private Projectile projectile = null;
    [SerializeField] private WeaponData weaponData = null;
    [SerializeField] private float attackCooldown = 1f;

    [Inject] private DiContainer container = null;
    [Inject] private Player player = null;

    private Transform playerTransform = null;
    private PlayerMovement movement = null;
    private IEnumerator attack = null;

    public void Activate()
    {
        enabled = true;
        StartCoroutine(attack);
    }

    public void Deactivate()
    {
        enabled = false;
        StopCoroutine(attack);
    }

    public void SetDefault()
    {
    }

    private void Awake()
    {
        attack = Attack();
        playerTransform = player.transform;
        movement = player.GetComponent<PlayerMovement>();
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            ProjectileStats projectileStats = weaponData.GetWeaponStats(0).ProjectileStats;

            float lead = 0f;
            Vector3 targetDirection = playerTransform.position - transform.position;

            Vector3 playerMovement = movement.Speed * playerTransform.forward;
            Vector3 orthogonalMovement = Vector3.Project(playerMovement, targetDirection);
            Vector3 tangentialMovement = playerMovement - orthogonalMovement;

            float distance = targetDirection.magnitude;
            float tangentialSpeed = tangentialMovement.magnitude;
            float targetOrthogonalSpeed = orthogonalMovement.magnitude;
            float rocketOrthogonalSpeed = Mathf.Sqrt(Mathf.Pow(projectileStats.Speed, 2) - Mathf.Pow(tangentialSpeed, 2));
            float timeToImpact = distance / (rocketOrthogonalSpeed - targetOrthogonalSpeed);

            Vector3 leadPosition = playerTransform.position + playerMovement * timeToImpact;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.up, transform.up); //Quaternion.LookRotation(targetPosition - transform.position, transform.up);
            Projectile createdProjectile = container.InstantiatePrefab(projectile, transform.position, targetRotation, null).GetComponent<Projectile>();
            createdProjectile.SetStats(projectileStats);

            SelfGuidance selfGuidance = createdProjectile.GetComponent<SelfGuidance>();
            if (selfGuidance)
            {
                selfGuidance.Target = playerTransform;
                selfGuidance.LeadDistance = lead;

                selfGuidance.TargetPosition = leadPosition;
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }
}