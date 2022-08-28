using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Campaign_", menuName = "Chapters/Campaign", order = 1)]
public class Campaign : ScriptableObject
{
    [Header("Display")]
    [SerializeField] private string campaignName = null;

    [Header("Data")]
    [SerializeField] private Level[] _levels;

    public string Name { get => campaignName; }

    public Level[] Levels { get => _levels; }
}