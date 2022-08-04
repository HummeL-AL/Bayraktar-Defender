using UnityEngine;

[CreateAssetMenu(fileName = "IntChannel_", menuName = "EventChannels/Int", order = 1)]
public class IntEventChannel : ScriptableObject
{
    public delegate void VoidEvent(int argument);
    public event VoidEvent ChannelEvent = null;

    public void RaiseEvent(int argument)
    {
        ChannelEvent?.Invoke(argument);
    }
}
