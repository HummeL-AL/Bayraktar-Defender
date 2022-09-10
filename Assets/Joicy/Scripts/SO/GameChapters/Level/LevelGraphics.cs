using UnityEngine;

[System.Serializable]
public class LevelGraphics
{
    [SerializeField] private Material fogMaterial = null;

    public Material Fog { get => fogMaterial; }
}
