using UnityEngine;
using UnityEngine.InputSystem;

public class CameraModesSwitcher : MonoBehaviour
{
    [SerializeField] private BoolEventChannel gameStateChanged = null;

    private CameraActionMap _actionMap = null;

    private void Awake()
    {
        _actionMap = new CameraActionMap();
    }

    private void OnEnable()
    {
        gameStateChanged.ChannelEvent += OnGameStateChanged;

        _actionMap?.Enable();
        _actionMap.Modes.ChooseNextMode.performed += SetNextMode;
        _actionMap.Modes.ChoosePreviousMode.performed += SetPreviousMode;
    }

    private void SetNextMode(InputAction.CallbackContext context)
    {

    }

    private void SetPreviousMode(InputAction.CallbackContext context)
    {

    }

    private void OnGameStateChanged(bool resumed)
    {
        if (resumed)
        {
            _actionMap?.Enable();
        }
        else
        {
            _actionMap?.Disable();
        }
    }

    private void OnDisable()
    {
        _actionMap?.Disable();
    }
}