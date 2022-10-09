using UnityEngine;


[CreateAssetMenu(fileName = "CellChannel_", menuName = "EventChannels/Cell", order = 1)]
public class CellEventChannel : ScriptableObject
{
    public delegate void VoidEvent(Cell cell);
    public event VoidEvent ChannelEvent = null;

    public void RaiseEvent(Cell cell)
    {
        ChannelEvent?.Invoke(cell);
    }
}
