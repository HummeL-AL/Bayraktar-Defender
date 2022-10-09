using Zenject;

public class VictimsDisplay : InfoDisplay
{
    [Inject] private LevelStats _stats = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        display.text = $"Victims: {_stats.Victims}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
