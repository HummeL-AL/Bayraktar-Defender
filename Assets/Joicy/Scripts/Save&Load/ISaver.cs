public interface ISaver
{
    public SaveData CreateNewSave();

    public void CreateSave(SaveData save);

    public SaveData LoadSave();
}