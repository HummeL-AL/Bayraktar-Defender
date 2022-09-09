using UnityEngine;

public class DirectMovement : MonoBehaviour, IEnemyMovement
{
    public IEnemyMovement.MovementDelegate TargetDistanceReached { get; set; }
    public float SpeedMultiplier { get; set; }

    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _targetDistance = 25f;

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    public void SetDefault()
    {
        SpeedMultiplier = 1;
    }

    public void Move()
    {
        MoveToCenter();
        CheckDistance();
    }

    private void Awake()
    {
        SetDefault();
    }

    private void Update()
    {
        Move();
    }

    private void MoveToCenter()
    {
        Vector3 toCenter = -transform.position.normalized;
        transform.Translate(toCenter * _speed * SpeedMultiplier * Time.deltaTime, Space.World);

        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up * 50f, Vector3.down, out hit, 100f, 1 << 6);

        transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        transform.LookAt(Vector3.zero, hit.normal);
    }

    private void CheckDistance()
    {
        if (transform.position.magnitude < _targetDistance)
        {
            TargetDistanceReached?.Invoke();
        }
    }
}
