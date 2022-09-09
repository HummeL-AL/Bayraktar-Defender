using Zenject;

public class AudioPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<AudioPlayer>().FromNew().AsSingle();
    }
}