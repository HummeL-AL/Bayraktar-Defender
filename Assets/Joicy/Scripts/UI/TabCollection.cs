using UnityEngine;

public class TabCollection : MonoBehaviour
{
    [SerializeField] Tab[] tabs = null;

    public void ActivateTab(int tabIndex)
    {
        CloseAllTabs();
        tabs[tabIndex].OpenTab();
    }

    private void CloseAllTabs()
    {
        foreach (var tab in tabs)
        {
            tab.CloseTab();
        }
    }
}