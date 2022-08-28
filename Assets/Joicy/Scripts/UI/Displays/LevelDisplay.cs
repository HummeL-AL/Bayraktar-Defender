using UnityEngine;
using Zenject;

public class LevelDisplay : MonoBehaviour
{
    public Campaign Campaign { get; set; }

    [SerializeField] private LevelChooser levelChooser = null;
    [Inject] private SaveData saveData = null;

    public void DisplayLevels()
    {
        Clear();

        int levelNumber = 0;
        foreach(Level level in Campaign.Levels)
        {
            int campaignID = level.CampaignNumber;
            bool isLevelAvailable = saveData.GameData.CompletedLevels[campaignID] >= levelNumber;

            LevelChooser chooser = Instantiate(levelChooser, transform);
            chooser.Level = level;
            chooser.Initialize(isLevelAvailable);

            levelNumber++;
        }
    }

    private void Clear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}