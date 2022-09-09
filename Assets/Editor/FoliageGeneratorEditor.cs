using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FoliageGenerator))]
[CanEditMultipleObjects]
public class FoliageGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FoliageGenerator foliageGenerator = (FoliageGenerator)target;

        if (GUILayout.Button("Generate foliage"))
        {
            foliageGenerator.GenerateFoliage();
        }

        if (GUILayout.Button("Clear"))
        {
            foliageGenerator.Clear();
        }
    }
}
