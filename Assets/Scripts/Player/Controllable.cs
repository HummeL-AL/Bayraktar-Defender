using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controllable : MonoBehaviour
{
    private bool _isControllable = true;
    private float _xSensivity = 1f;
    private float _ySensivity = 1f;

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

    public void SetStats(ControllableStats stats)
    {
        _xSensivity = stats.Sensivity.x;
        _ySensivity = stats.Sensivity.y;
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
        Vector3 correction = new Vector3(delta.y * _ySensivity, delta.x * _xSensivity, 0f);

        Quaternion correctionRotation = Quaternion.Euler(correction * Time.deltaTime);
        _rigidbody.MoveRotation(transform.rotation * correctionRotation);
    }
}
