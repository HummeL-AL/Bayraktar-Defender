using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LandscapeGenerator))]
[CanEditMultipleObjects]
public class LandscapeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LandscapeGenerator landscapeGenerator = (LandscapeGenerator)target;

        if (GUILayout.Button("Generate landscape"))
        {
            landscapeGenerator.SetMesh(landscapeGenerator.GenerateMesh());
        }
    }
}
