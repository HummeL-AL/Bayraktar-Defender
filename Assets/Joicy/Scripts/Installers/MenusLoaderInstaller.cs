using UnityEngine;
using Zenject;

public class MenusLoaderInstaller : MonoInstaller
{
    [SerializeField] private MenusLoader menusLoader = null;

    public override void InstallBindings()
    {
        Container.Bind<MenusLoader>().FromInstance(menusLoader).AsSingle();
    }
}