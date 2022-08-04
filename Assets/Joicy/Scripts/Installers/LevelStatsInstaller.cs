using UnityEngine;
using Zenject;

public class LevelStatsInstaller : MonoInstaller
{
    [SerializeField] private LevelStats levelStats = null;

    public override void InstallBindings()
    {
        Container.Bind<LevelStats>().FromInstance(levelStats).AsSingle();
    }
}