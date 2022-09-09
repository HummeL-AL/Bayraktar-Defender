using Zenject;

public class SettingsDataInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SettingsData>().FromNew().AsSingle();
    }
}