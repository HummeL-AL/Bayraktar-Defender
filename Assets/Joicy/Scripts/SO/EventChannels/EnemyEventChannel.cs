using UnityEngine;

[CreateAssetMenu(fileName = "EnemyChannel_", menuName = "EventChannels/Enemy", order = 1)]
public class EnemyEventChannel : ScriptableObject
{
    public delegate void VoidEvent(Enemy argument);
    public event VoidEvent ChannelEvent = null;

    public void RaiseEvent(Enemy argument)
    {
        ChannelEvent?.Invoke(argument);
    }
}
