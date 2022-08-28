using Zenject;

public class SaveDataInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SaveData>().FromNew().AsSingle();
    }
}