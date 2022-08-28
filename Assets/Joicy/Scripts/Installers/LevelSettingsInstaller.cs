using UnityEngine;
using Zenject;

public class LevelSettingsInstaller : MonoInstaller
{
    [SerializeField] private Level levelSettings = null;

    public override void InstallBindings()
    {
        Container.Bind<Level>().FromInstance(levelSettings).AsSingle();
    }
}