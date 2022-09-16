using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelGraphics
{
    [SerializeField] private Material fogMaterial = null;
    [SerializeField] public List<GeneratedObjectType> ObjectsTypes = new List<GeneratedObjectType>();

    public Material Fog { get => fogMaterial; }
}

[System.Serializable]
public class GeneratedObjectType
{
    public Mesh Mesh { get; private set; }
    public Material Material { get; private set; }
    public List<Matrix4x4[]> Transforms { get; private set; }

    public void SetValues(Mesh mesh, Material material, Matrix4x4[] transforms)
    {
        SetModel(mesh, material);
        SetTransforms(transforms);
    }

    public void SetModel(Mesh mesh, Material material)
    {
        Mesh = mesh;
        Material = material;
    }

    public void SetTransforms(Matrix4x4[] transforms)
    {
        Transforms = new List<Matrix4x4[]>();

        List<Matrix4x4> matrices = new List<Matrix4x4>();
        int matrixIndex = 0;

        for (int i = 0; i < transforms.Length; i++)
        {
            if(matrixIndex < 1024)
            {
                matrices.Add(transforms[i]);
            }
            else
            {
                matrixIndex = 0;
                Transforms.Add(matrices.ToArray());
                matrices.Clear();
            }

            matrixIndex++;
        }

        Transforms.Add(matrices.ToArray());
        matrices.Clear();
    }
}
