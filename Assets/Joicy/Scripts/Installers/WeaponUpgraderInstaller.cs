using UnityEngine;
using Zenject;

public class WeaponUpgraderInstaller : MonoInstaller
{
    [SerializeField] private WeaponUpgrader weaponUpgrader = null;

    public override void InstallBindings()
    {
        Container.Bind<WeaponUpgrader>().FromInstance(weaponUpgrader).AsSingle();
    }
}