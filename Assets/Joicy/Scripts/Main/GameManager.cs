using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoolEventChannel gameStateChanged = null;

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        gameStateChanged.RaiseEvent(false);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        AudioListener.pause = false;
        gameStateChanged.RaiseEvent(true);
    }
}
