using UnityEngine;

[CreateAssetMenu(fileName = "FoliageObject_", menuName = "ScriptableObjects/FoliageObject_", order = 1)]
public class FoliageObject : ScriptableObject
{
    [SerializeField] private GameObject objectPrefab = null;
    [SerializeField] private float minimalDistance = 2f;
    [SerializeField] private Material material = null;
    [SerializeField] private Mesh[] MeshLODs = null;

    public GameObject ObjectPrefab { get => objectPrefab; }
    public float MinimalDistance { get => minimalDistance; }
    public Material Material { get => material; }
    public Mesh[] LODs { get => MeshLODs; }
}