using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class CampaignChooser : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown chooser = null;
    [SerializeField] private LevelDisplay levelDisplay = null;

    [Inject] private ResourcesLoader _loader;

    public void UpdateCampaign()
    {
        ChooseCampaign(GetValue());
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        List<string> options = new List<string>();

        foreach(Campaign campaign in _loader.Campaigns)
        {
            options.Add(campaign.Name);
        }
        chooser.ClearOptions();
        chooser.AddOptions(options);

        ChooseCampaign(0);
    }

    private void ChooseCampaign(int index)
    {
        levelDisplay.Campaign = _loader.Campaigns[index];
        levelDisplay.DisplayLevels();
    }

    private int GetValue()
    {
        return chooser.value;
    }
}
