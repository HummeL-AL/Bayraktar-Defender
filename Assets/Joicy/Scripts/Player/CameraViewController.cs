using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class CameraViewController : MonoBehaviour
{
    [SerializeField] private VoidEventChannel settingsChangedChannel = null;

    [SerializeField] private Camera mainCamera = null;
    [SerializeField] private float _zoomSensitivity = 1f;
    [SerializeField] private float _rotatingSensitivity = 1f;
    [SerializeField] private Vector3 _minAngles = Vector3.zero;
    [SerializeField] private Vector3 _maxAngles = Vector3.one;
    [SerializeField] private float _minZoom = 30;
    [SerializeField] private float _maxZoom = 60;

    [Inject] private SettingsData settingsData = null;

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

    public void SetStats(ZoomUpgrade stats)
    {
        _minZoom = stats.MinZoom;
        _maxZoom = stats.MaxZoom;

    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;
        settingsChangedChannel.ChannelEvent += OnSettingsChanged;

        ApplySettings();
    }

    private void UpdateZoom(float delta)
    {
        float currentZoom = mainCamera.fieldOfView - delta * _zoomSensitivity;
        mainCamera.fieldOfView = Mathf.Clamp(currentZoom, _minZoom, _maxZoom);
    }

    private void UpdateRotating(Vector2 delta)
    {
        float sensitivity = _rotatingSensitivity * mainCamera.fieldOfView;
        Vector3 moving = new Vector3(delta.y * sensitivity, delta.x * sensitivity, 0f);
        Transform camera = mainCamera.transform;

        Vector3 currentAngles = camera.localEulerAngles + moving;
        currentAngles.x = Mathf.Clamp(currentAngles.x, _minAngles.x, _maxAngles.x);
        currentAngles.y = Mathf.Clamp(currentAngles.y, _minAngles.y, _maxAngles.y);

        camera.localEulerAngles = currentAngles;
    }

    private void ApplySettings()
    {
        _rotatingSensitivity = Mathf.Lerp(0.0005f, 0.005f, settingsData.Control.Sensitivity);
    }

    private void OnSettingsChanged()
    {
        ApplySettings();
    }
}
