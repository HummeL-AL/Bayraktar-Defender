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
            if (OpenAvailable())
            {
                OpenTab();
                OnTabOpen.Invoke();
            }
        }
        else
        {
            CloseTab();
            OnTabClosed.Invoke();
        }
    }

    private void OpenTab()
    {
        gameObject.SetActive(true);
    }

    private void CloseTab()
    {
        CloseChilds();
        gameObject.SetActive(false);
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
