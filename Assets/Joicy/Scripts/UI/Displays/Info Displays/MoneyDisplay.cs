using UnityEngine;
using Zenject;
using TMPro;

public class MoneyDisplay : MonoBehaviour
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
        display.text = $"$: {_stats.Money}";
    }

    private void OnDestroy()
    {
        _eventChannel.ChannelEvent -= UpdateValue;
    }
}
