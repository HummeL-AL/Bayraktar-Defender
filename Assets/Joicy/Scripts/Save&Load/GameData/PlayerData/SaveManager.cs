using System;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    [Inject] private ResourcesLoader resourcesLoader = null;
    [Inject] private DiContainer container = null;
    [Inject] private SaveData data = null;

    public void CreateNewSave()
    {
        if (Utility.IsConnected())
        {
            Saver onlineSaver = new DatabaseManager(resourcesLoader);
            data.CopyValues(onlineSaver.CreateNewSaveData());
        }

        if (!(Application.platform == RuntimePlatform.WebGLPlayer))
        {
            Saver localSaver = new LocalSaveManager(resourcesLoader);
            data.CopyValues(localSaver.CreateNewSaveData());
        }

        UpdateSave();
    }

    public void CreateSave()
    {
        string saveDate = Utility.GetDate();
        data.SaveTime = saveDate;

        if(Utility.IsConnected())
        {
            Saver onlineSaver = new DatabaseManager(resourcesLoader);
            onlineSaver.CreateSave(data);
        }

        if(!(Application.platform == RuntimePlatform.WebGLPlayer))
        {
            Saver localSaver = new LocalSaveManager(resourcesLoader);
            localSaver.CreateSave(data);
        }
    }

    public void LoadSave()
    {
        SaveData localData = null;
        SaveData onlineData = null;

        if (Utility.IsConnected())
        {
            Saver onlineSaver = new DatabaseManager(resourcesLoader);
            localData = onlineSaver.LoadSave();
        }

        if (!(Application.platform == RuntimePlatform.WebGLPlayer))
        {
            Saver localSaver = new LocalSaveManager(resourcesLoader);
            onlineData = localSaver.LoadSave();
        }

        if(localData == null)
        {
            data.CopyValues(onlineData);
        }
        else if(onlineData == null)
        {
            data.CopyValues(localData);
        }
        else
        {
            long localSaveTime = Convert.ToInt64(localData.SaveTime);
            long onlineSaveTime = Convert.ToInt64(onlineData.SaveTime);

            if(localSaveTime > onlineSaveTime)
            {
                data.CopyValues(localData);
            }
            else
            {
                data.CopyValues(onlineData);
            }
        }

        UpdateSave();
    }

    private void UpdateSave()
    {
        container.Bind<SaveData>().FromInstance(data).AsSingle();
    }
}