using UnityEngine;

[System.Serializable]
public struct DisplayStats
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    public string Name { get => name; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
}
