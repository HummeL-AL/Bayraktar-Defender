using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level_", menuName = "Chapters/Level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] private string levelName = null;
    [SerializeField] private int campaignNumber = 0;
    [SerializeField] private int levelNumber = 0;

    [SerializeField] private int victimsPenalty = 0;

    [SerializeField] private Wave[] _waves = null;

    public string Name { get => levelName; }
    public int CampaignNumber { get => campaignNumber; }
    public int LevelNumber { get => levelNumber; }
    public string Scene { get => $"Level_{campaignNumber}_{levelNumber}"; }

    public int VictimsPenalty { get => victimsPenalty; }

    public Wave[] Waves { get => _waves; }
}