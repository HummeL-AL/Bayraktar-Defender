using UnityEngine;
using UnityEngine.Events;

public class Tab : MonoBehaviour
{
    [SerializeField] private Tab[] _blockerTabs = null;
    [SerializeField] private Tab[] _childTabs = null;

    public UnityEvent OnTabOpen = null;
    public UnityEvent OnTabClosed = null;

    public void ActivateTab()
    {
        if (!gameObject.activeSelf)
        {
            OpenTab();
        }
        else
        {
            CloseTab();
        }
    }

    public void OpenTab()
    {
        if (OpenAvailable())
        {
            gameObject.SetActive(true);
            OnTabOpen.Invoke();
        }
    }

    public void CloseTab()
    {
        if (gameObject.activeSelf)
        {
            CloseChilds();
            gameObject.SetActive(false);
            OnTabClosed.Invoke();
        }
    }

    private bool OpenAvailable()
    {
        bool available = true;

        foreach (Tab blocker in _blockerTabs)
        {
            if(blocker.gameObject.activeSelf)
            {
                available = false;
            }
        }

        return available;
    }

    private void CloseChilds()
    {
        if (_childTabs != null)
        {
            foreach (Tab tab in _childTabs)
            {
                tab.CloseTab();
            }
        }
    }
}
