using UnityEngine;

public class SpeedDisplay : InfoDisplay
{
    [SerializeField] private PlayerMovement _player = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        string speed = string.Format("{0:0.0}", _player.Speed);
        display.text = $"SPD:{speed}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
