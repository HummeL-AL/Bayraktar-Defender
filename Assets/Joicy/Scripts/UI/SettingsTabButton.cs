using UnityEngine;
using TMPro;

public class SettingsTabButton : MonoBehaviour
{
    [SerializeField] protected SettingsDisplay display = null;
    [SerializeField] protected TMP_Text tabName = null;

    protected int tabID = 0;

    public void Initialize(SettingsDisplay targetDisplay, int index, string name)
    {
        display = targetDisplay;
        tabID = index;
        tabName.text = name;
    }

    public void SwitchTab()
    {
        display.DisplaySettings(tabID);
    }
}