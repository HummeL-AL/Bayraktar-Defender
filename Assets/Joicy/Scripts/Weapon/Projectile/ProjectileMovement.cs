using UnityEngine;

public class ProjectileMovement : MonoBehaviour, IProjectileComponent
{
    public delegate void CollisionDelegate();
    public CollisionDelegate ObstacleDetected;

    [SerializeField] private float speed = 0;

    private Rigidbody rigidbody = null;

    public void SetStats(ProjectileStats projectileStats)
    {
        speed = projectileStats.Speed;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckHit();
    }

    private void Move()
    {
        rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void CheckHit()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, speed * Time.fixedDeltaTime, 1 << 6 | 1 << 8);

        if (hit.collider)
        {
            ObstacleDetected.Invoke();
        }
    }
}
