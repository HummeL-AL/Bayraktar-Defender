using Zenject;

public class EnemiesDisplay : InfoDisplay
{
    [Inject] private LevelStats _stats = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        display.text = $"ENM:{_stats.EnemiesCount}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
