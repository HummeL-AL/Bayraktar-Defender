using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LocalSaveManager : Saver
{
    private string _path = $"{Application.persistentDataPath}/save.json";

    public LocalSaveManager(ResourcesLoader newResourcesLoader) : base(newResourcesLoader)
    {
        resourcesLoader = newResourcesLoader;
    }

    public override void CreateSave(SaveData save)
    {
        string json = JsonConvert.SerializeObject(save);

        StreamWriter writer = new StreamWriter(_path, false);
        writer.BaseStream.Seek(0, SeekOrigin.Begin);
        writer.Write(json);
        writer.Flush();
        writer.Close();
    }

    public override SaveData LoadSave()
    {
        SaveData save = null;

        if (File.Exists(_path))
        {
            string json;

            FileStream stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            json = reader.ReadToEnd();
            reader.Close();

            save = JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            save = CreateNewSaveData();
        }

        return save;
    }

}
