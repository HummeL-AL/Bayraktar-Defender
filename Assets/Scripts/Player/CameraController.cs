using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _zoomSensivity = 1f;
    [SerializeField] private float _rotatingSensivity = 1f;
    [SerializeField] private Vector3 _minAngles = Vector3.zero;
    [SerializeField] private Vector3 _maxAngles = Vector3.one;
    [SerializeField] private float _minZoom = 30;
    [SerializeField] private float _maxZoom = 60;

    public void OnZooming(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();
        if (delta != 0)
        {
            UpdateZoom(delta);
        }
    }

    public void OnRotating(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();
        if(delta != Vector2.zero)
        {
            UpdateRotating(delta);
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateZoom(float delta)
    {
        float currentZoom = Camera.main.fieldOfView - delta * _zoomSensivity;
        Camera.main.fieldOfView = Mathf.Clamp(currentZoom, _minZoom, _maxZoom);
    }

    private void UpdateRotating(Vector2 delta)
    {
        Vector3 moving = new Vector3(delta.y * _rotatingSensivity, delta.x * _rotatingSensivity, 0f);

        Vector3 currentAngles = transform.localEulerAngles + moving;
        currentAngles.x = Mathf.Clamp(currentAngles.x, _minAngles.x, _maxAngles.x);
        currentAngles.y = Mathf.Clamp(currentAngles.y, _minAngles.y, _maxAngles.y);

        transform.localEulerAngles = currentAngles;
    }
}
