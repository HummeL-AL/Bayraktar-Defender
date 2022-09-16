using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Foliage))]
[CanEditMultipleObjects]
public class FoliageGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Foliage foliageGenerator = (Foliage)target;

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
