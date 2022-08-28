using Zenject;

public class ResourcesLoaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ResourcesLoader>().FromNew().AsSingle();
    }
}
