using System.Collections;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour, IProjectileDataReceiver
{
    public delegate void CollisionDelegate(RaycastHit hit);
    public CollisionDelegate ObstacleDetected;

    [SerializeField] private float speed = 0;
    [SerializeField] private LayerMask collidedLayers = 0;

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
        StartCoroutine(CheckHit());
        Move();
    }

    private void Move()
    {
        rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private IEnumerator CheckHit()
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, speed * Time.fixedDeltaTime, collidedLayers);

        if (hit.collider)
        {
            yield return new WaitForFixedUpdate();
            transform.position = hit.point;
            ObstacleDetected.Invoke(hit);
        }
    }
}
