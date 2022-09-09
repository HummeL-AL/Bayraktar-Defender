using UnityEngine;

public interface IModelSwitcher
{
    LODGroup[] Models { get; set; }
    LODSwitcher Switcher { get; set; }

    public void SetModel()
    {
        if(Models != null && Models.Length > 1)
        {
            int modelIndex = Random.Range(1, Models.Length);
            LODGroup model = Models[modelIndex];

            Switcher.SwitchLOD(model);
        }
    }

    public void SetModel(int index)
    {
        if (Models != null && Models.Length > 0)
        {
            LODGroup model = Models[index];

            Switcher.SwitchLOD(model);
        }
    }
}
