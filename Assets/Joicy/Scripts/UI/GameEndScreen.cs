using UnityEngine;
using TMPro;

[RequireComponent(typeof(Tab))]
public class GameEndScreen : MonoBehaviour
{
    [SerializeField] private VoidEventChannel gameWonChannel = null;
    [SerializeField] private VoidEventChannel gameLostChannel = null;

    [SerializeField] private TMP_Text gameResult = null;
    private Tab tab = null;

    private void Awake()
    {
        gameWonChannel.ChannelEvent += ShowWonScreen;
        gameLostChannel.ChannelEvent += ShowLoseScreen;
        tab = GetComponent<Tab>();

        tab.CloseTab();
    }

    private void ShowWonScreen()
    {
        tab.OpenTab();
        gameResult.text = "YOU WON!";
    }

    private void ShowLoseScreen()
    {
        tab.OpenTab();
        gameResult.text = "YOU LOSE!";
    }

    private void OnDestroy()
    {
        gameWonChannel.ChannelEvent -= ShowWonScreen;
        gameLostChannel.ChannelEvent -= ShowLoseScreen;
    }
}
