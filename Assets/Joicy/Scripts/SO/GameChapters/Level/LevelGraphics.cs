using UnityEngine;

[System.Serializable]
public class LevelGraphics
{
    [SerializeField] private Material fogMaterial = null;
    [SerializeField] private FoliageInstance[] foliageInstances = null;

    public Material Fog { get => fogMaterial; }
    public FoliageInstance[] FoliageInstances { get => foliageInstances; set => foliageInstances = value; }
}

[System.Serializable]
public struct FoliageInstance
{
    public Vector3 Position;
    public Quaternion Rotation;
    public FoliageObject ObjectData;
    public Vector2 Position2D
    {
        get => new Vector2(Position.x, Position.z);
        set => Position = new Vector3(value.x, Position.y, value.y);
    }
}