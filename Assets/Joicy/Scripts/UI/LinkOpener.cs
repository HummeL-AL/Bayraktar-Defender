using UnityEngine;

public class LinkOpener : MonoBehaviour
{
    [SerializeField] private string link = null;

    public void OpneLink()
    {
        Application.OpenURL(link);
    }
}
