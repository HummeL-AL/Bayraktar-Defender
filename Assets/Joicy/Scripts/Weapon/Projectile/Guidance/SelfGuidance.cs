using UnityEngine;

public class SelfGuidance : MonoBehaviour, IProjectileComponent
{
    public Transform Target = null;
    public float LeadDistance = 0f;
    public float RotationSpeed = 5f;

    private Rigidbody rigidbody = null;

    public void SetStats(ProjectileStats projectileStats)
    {
        RotationSpeed = projectileStats.Sensivity;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        Vector3 targetPoint = GetTargetPoint();
        Vector3 relativeDirection = targetPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, transform.up);
        Quaternion availableRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        //Quaternion availableRotation = Quaternion.FromToRotation(transform.forward, Vector3.up);

        rigidbody.MoveRotation(availableRotation);
    }

    private Vector3 GetTargetPoint()
    {
        return Target.position + Target.forward * LeadDistance;
    }
}