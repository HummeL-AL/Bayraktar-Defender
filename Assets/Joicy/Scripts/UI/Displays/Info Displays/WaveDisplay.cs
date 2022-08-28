using UnityEngine;
using Zenject;
using TMPro;

public class WaveDisplay : MonoBehaviour
{
    [SerializeField] private VoidEventChannel _eventChannel = null;

    [Inject] private LevelStats _stats = null;
    private TMP_Text display = null;

    private void Awake()
    {
        display = GetComponent<TMP_Text>();
        _eventChannel.ChannelEvent += UpdateValue;
    }

    private void UpdateValue()
    {
        display.text = $"WAVE:{_stats.Wave}";
    }
}