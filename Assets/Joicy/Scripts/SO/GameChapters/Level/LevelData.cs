using UnityEngine;

[System.Serializable]
public class LevelData
{
    [SerializeField] private string levelName = null;
    [SerializeField] private int campaignNumber = 0;
    [SerializeField] private int levelNumber = 0;

    public string Name { get => levelName; }
    public int CampaignNumber { get => campaignNumber; }
    public int LevelNumber { get => levelNumber; }
    public string Scene { get => $"Level_{campaignNumber}_{levelNumber}"; }
}
