using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class InfoDisplay : MonoBehaviour
{
    [SerializeField] protected VoidEventChannel _eventChannel = null;
    protected TMP_Text display = null;

    protected virtual void Awake()
    {
        _eventChannel.ChannelEvent += UpdateValue;
        display = GetComponent<TMP_Text>();
    }

    protected virtual void OnDestroy()
    {
        _eventChannel.ChannelEvent -= UpdateValue;
    }

    protected virtual void UpdateValue()
    {

    }
}
