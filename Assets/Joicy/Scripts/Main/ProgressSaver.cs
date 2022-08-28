using UnityEngine;
using Zenject;

public class ProgressSaver : MonoBehaviour
{
   [Inject] private SaveManager saveManager = null;

    private void OnEnable()
    {
        saveManager.CreateSave();
    }
}