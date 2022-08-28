using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player player = null;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle();
    }
}