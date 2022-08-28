using UnityEngine;
using Zenject;

public class SaveManagerInstaller : MonoInstaller
{
    [SerializeField] private SaveManager saveManager = null;

    public override void InstallBindings()
    {
        Container.Bind<SaveManager>().FromInstance(saveManager).AsSingle();
    }
}