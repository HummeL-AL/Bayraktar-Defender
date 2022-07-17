using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _targetDistance = 25f;

    private void Start()
    {
        StartCoroutine(MoveToDistance(_targetDistance));
    }

    private void MoveToCenter()
    {
        Vector3 toCenter = -transform.position.normalized;
        transform.Translate(toCenter * _speed * Time.deltaTime, Space.World);

        RaycastHit hit;
        Physics.Raycast(transform.position + Vector3.up * 50f, Vector3.down, out hit, 100f, 1 << 6);

        transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        transform.LookAt(Vector3.zero, hit.normal); //Quaternion.FromToRotation(Vector3.up, hit.normal);
    }

    private IEnumerator MoveToDistance(float targetDistance)
    {
        bool moving = true;
        while (moving)
        {
            if (transform.position.magnitude > targetDistance)
            {
                MoveToCenter();
                yield return new WaitForEndOfFrame();
            }
            else
            {
                moving = false;

                foreach(EnemyRole role in gameObject.GetComponents<EnemyRole>())
                {
                    role.enabled = true;
                }
            }
        }
    }
}
