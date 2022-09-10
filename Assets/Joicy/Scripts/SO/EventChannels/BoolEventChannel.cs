using UnityEngine;

[CreateAssetMenu(fileName = "BoolChannel_", menuName = "EventChannels/Bool", order = 1)]
public class BoolEventChannel : ScriptableObject
{
    public delegate void VoidEvent(bool argument);
    public event VoidEvent ChannelEvent = null;

    public void RaiseEvent(bool argument)
    {
        ChannelEvent?.Invoke(argument);
    }
}
