using Zenject;

public class MoneyDisplay : InfoDisplay
{
    [Inject] private LevelStats _stats = null;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateValue()
    {
        display.text = $"$: {_stats.Money}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
