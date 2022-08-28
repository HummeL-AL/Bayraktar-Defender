using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controllable : MonoBehaviour, IProjectileComponent
{
    private bool _isControllable = true;
    private float sensivity = 1f;

    private Rigidbody _rigidbody = null;

    public void OnCorrection(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        if (delta != Vector2.zero)
        {
            StartCoroutine("Correction", delta);
        }
        else
        {
            StopCoroutine("Correction");
        }
    }

    public void SetStats(ProjectileStats projectileStats)
    {
        sensivity = projectileStats.Sensivity;
    }

    public void SetControllable(bool isControllable)
    {
        _isControllable = isControllable;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private IEnumerator Correction(Vector2 delta)
    {
        while (true)
        {
            if (_isControllable)
            {
                ApplyCorrection(delta);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void ApplyCorrection(Vector2 delta)
    {
        Vector3 correction = new Vector3(delta.y * sensivity, delta.x * sensivity, 0f);

        Quaternion correctionRotation = Quaternion.Euler(correction * Time.deltaTime);
        _rigidbody.MoveRotation(transform.rotation * correctionRotation);
    }
}
