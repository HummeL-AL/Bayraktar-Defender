using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInput[] _inputs = null;
    [SerializeField] private PlayerInput _playerInput = null;
    
    public static bool GamePaused { get; private set; }

    public void Pause()
    {
        _playerInput.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GamePaused = true;
    }

    public void Resume()
    {
        _playerInput.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GamePaused = false;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        EnableInputs();
    }

    private void EnableInputs()
    {
        foreach (PlayerInput input in _inputs)
        {
            input.enabled = true;
        }
    }
}
