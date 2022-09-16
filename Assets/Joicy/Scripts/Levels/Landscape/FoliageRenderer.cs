using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

[ExecuteAlways]
public class FoliageRenderer : MonoBehaviour
{
    [SerializeField] private int renderLayer = 0;

    [SerializeField] [Inject] private Level LevelInfo = null;

    private LevelGraphics levelGraphics = null;

    private void Awake()
    {
        levelGraphics = LevelInfo.LevelGraphics;
    }

    private void Update()
    {
        DrawInstanced(LevelInfo.LevelGraphics.ObjectsTypes);
    }

    private void DrawInstanced(List<GeneratedObjectType> objectTypes)
    {
        foreach (GeneratedObjectType objectType in objectTypes)
        {
            foreach (Matrix4x4[] drawCall in objectType.Transforms)
            {
                Graphics.DrawMeshInstanced(
                    objectType.Mesh,
                    0,
                    objectType.Material,
                    drawCall,
                    drawCall.Length,
                    null,
                    ShadowCastingMode.On,
                    true,
                    renderLayer,
                    null
                );
            }
        }

        //Tutorial used strange method with rendering random chunks of objects:
        //GeneratedObjectType objectType = objectTypes[Random.Range(0, objectTypes.Count)];
        //Matrix4x4[] drawCall = objectType.Transforms[Random.Range(0, objectType.Transforms.Count)];

        //Debug.Log($"Types: {objectTypes.Count}");
        //Debug.Log($"{objectType.Transforms.Count} objects in {objectType} type");

        //Graphics.DrawMeshInstanced(
        //    objectType.Mesh,
        //    0,
        //    objectType.Material,
        //    drawCall,
        //    drawCall.Length,
        //    null,
        //    ShadowCastingMode.On,
        //    true,
        //    renderLayer,
        //    null
        //);
    }
}
