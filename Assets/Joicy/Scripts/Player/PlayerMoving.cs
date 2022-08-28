using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMoving : MonoBehaviour
{
    public UnityEvent OnMove;

    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 3f;
    [SerializeField] private float _accelerationSpeed = 0.5f;
    [SerializeField] private float _speed = 1f;

    [SerializeField] AudioSource _audioSource = null;
    [SerializeField] Vector2 _minMaxVolume = Vector2.one;

    public float Speed { get => _speed; }

    public void OnMoving(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>() * _accelerationSpeed;
        if (context.phase == InputActionPhase.Started)
        {
            StartCoroutine("Accelerating", delta);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            StopCoroutine("Accelerating");
        }
    }

    private void FixedUpdate()
    {
        transform.parent.rotation *= Quaternion.Euler(0f, -_speed * Time.fixedDeltaTime, 0f);
    }

    private IEnumerator Accelerating(float delta)
    {
        while (true)
        {
            ChangeSpeed(delta);
            yield return new WaitForEndOfFrame();
        }
    }

    private void ChangeSpeed(float delta)
    {
        _speed = Mathf.Clamp(_speed + (delta * Time.deltaTime), _minSpeed, _maxSpeed);

        float volumePercent = (_speed - _minSpeed) / (_maxSpeed - _minSpeed);
        _audioSource.volume = _minMaxVolume.x + (_minMaxVolume.y - _minMaxVolume.x) * volumePercent;
        OnMove.Invoke();
    }
}
