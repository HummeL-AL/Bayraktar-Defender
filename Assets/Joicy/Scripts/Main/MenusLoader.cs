using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusLoader : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene($"{name}", LoadSceneMode.Single);
    }
}