using UnityEngine;
using Zenject;

public class SceneTransitor : MonoBehaviour
{
    [SerializeField] private string targetMenu = null;

    [Inject] private MenusLoader menusLoader = null;

    public void LoadMenu()
    {
        menusLoader.LoadScene(targetMenu);
    }
}