using UnityEngine;
using Zenject;

public class CityInstaller : MonoInstaller
{
    [SerializeField] private City city = null;

    public override void InstallBindings()
    {
        Container.Bind<City>().FromInstance(city).AsSingle();
    }
}