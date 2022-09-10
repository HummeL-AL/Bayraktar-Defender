using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Level_", menuName = "Chapters/Level", order = 1)]
public class Level : ScriptableObject
{
    [SerializeField] private LevelData levelData = null;
    [SerializeField] private LevelGraphics levelGraphics = null;
    [SerializeField] private LevelGameplay levelGameplay = null;

    public LevelData LevelData { get => levelData; }
    public LevelGraphics LevelGraphics { get => levelGraphics; }
    public LevelGameplay LevelGameplay { get => levelGameplay; }
}