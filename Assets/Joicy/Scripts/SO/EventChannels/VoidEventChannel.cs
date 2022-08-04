using UnityEngine;


[CreateAssetMenu(fileName = "VoidChannel_", menuName = "EventChannels/Void", order = 1)]
public class VoidEventChannel : ScriptableObject
{
    public delegate void VoidEvent();
    public event VoidEvent ChannelEvent = null;

    public void RaiseEvent()
    {
        ChannelEvent?.Invoke();
    }
}
