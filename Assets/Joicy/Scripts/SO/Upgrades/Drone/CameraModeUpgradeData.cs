using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "CameraModeUpgrade_", menuName = "ScriptableObjects/DroneUpgrades/CameraMode", order = 1)]
public class CameraModeUpgradeData : DroneUpgradeData
{
    [SerializeField] private bool isVisible = true;
    [SerializeField] private bool unlockedByDefault = false;
    [SerializeField] private int rendererDataIndex = 0;
    [SerializeField] private UniversalRendererData rendererData = null;
    [SerializeField] private CameraModeUpgrade[] upgrades;

    public override bool IsVisible { get => isVisible; }
    public override bool UnlockedByDefault { get => unlockedByDefault; }
    public override string Name { get => name; }
    public override int MaxLevel { get => upgrades.Length - 1; }
    public int RendererDataIndex { get => rendererDataIndex; }
    public UniversalRendererData RendererData { get => rendererData; }

    public override IDroneUpgrade GetUpgrade(int level)
    {
        return upgrades[level];
    }
}

[System.Serializable]
public class CameraModeUpgrade : IDroneUpgrade
{
    [SerializeField] private string upgradeName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    [SerializeField] private float renderScale = 1f;

    [SerializeField] private int price;

    public string Name { get => upgradeName; }
    public string Description { get => description; }
    public float RenderScale { get => renderScale; }
    public Sprite Icon { get => icon; }
    public int Price { get => price; }
}