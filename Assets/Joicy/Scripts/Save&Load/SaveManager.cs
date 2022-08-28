using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] private ResourcesLoader resourcesLoader = null;
    [Inject] private DiContainer container = null;
    [Inject] private SaveData data = null;

    public void CreateNewSave()
    {
        ISaver saver = GetSaver();
        data.CopyValues(saver.CreateNewSave());
        UpdateSave();
    }

    public void CreateSave()
    {
        ISaver saver = GetSaver();
        saver.CreateSave(data);
    }

    public void LoadSave()
    {
        ISaver saver = GetSaver();
        data.CopyValues(saver.LoadSave());
        UpdateSave();
    }

    private void UpdateSave()
    {
        container.Bind<SaveData>().FromInstance(data).AsSingle();
    }

    private ISaver GetSaver()
    {
        if (Utility.IsConnected())
        {
            return new LocalSaveManager(resourcesLoader);
            //return new DatabaseManager();
        }
        else
        {
            return new LocalSaveManager(resourcesLoader);
        }
    }
}