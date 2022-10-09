using Zenject;

public class WaveDisplay : InfoDisplay
{
    [Inject] private LevelStats _stats = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        display.text = $"WAVE:{_stats.Wave}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
