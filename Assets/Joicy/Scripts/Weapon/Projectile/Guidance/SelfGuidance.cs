using UnityEngine;

public class SelfGuidance : MonoBehaviour, IProjectileDataReceiver
{
    public Transform Target = null;
    public Vector3 TargetPosition = Vector3.zero;
    public float LeadDistance = 0f;
    public float RotationSpeed = 5f;

    [SerializeField] private bool followLead = false;
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
        Vector3 targetPoint = Vector3.zero;
        if (followLead)
        {
            targetPoint = GetTargetPoint();
        }
        else
        {
            targetPoint = TargetPosition;
        }

        Vector3 relativeDirection = targetPoint - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativeDirection, transform.up);
        Quaternion availableRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

        rigidbody.MoveRotation(availableRotation);
    }

    private Vector3 GetTargetPoint()
    {
        return Target.position + Target.forward * LeadDistance;
    }
}