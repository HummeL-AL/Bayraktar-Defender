using UnityEngine;

public class Controllable : MonoBehaviour, IProjectileDataReceiver
{

    private bool _isControllable = true;
    private float _sensitivity = 1f;

    private ProjectileActionMap _actionMap = null;
    private Rigidbody _rigidbody = null;

    public void SetStats(ProjectileStats projectileStats)
    {
        _sensitivity = projectileStats.Sensivity;
    }

    public void SetControllable(bool isControllable)
    {
        _isControllable = isControllable;
    }

    private void Awake()
    {
        _actionMap = new ProjectileActionMap();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _actionMap?.Enable();
    }

    private void Update()
    {
        ApplyCorrection(_actionMap.Correctable.ProjectileCorrection.ReadValue<Vector2>());
    }

    private void ApplyCorrection(Vector2 delta)
    {
        if (delta != Vector2.zero && _isControllable)
        {
            Vector3 correction = new Vector3(delta.y * _sensitivity, delta.x * _sensitivity, 0f);

            Quaternion correctionRotation = Quaternion.Euler(correction * Time.deltaTime);
            _rigidbody.MoveRotation(transform.rotation * correctionRotation);
        }
    }

    private void OnDisable()
    {
        _actionMap?.Disable();
    }
}
