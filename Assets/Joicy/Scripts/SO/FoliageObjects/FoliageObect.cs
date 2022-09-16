using UnityEngine;

[CreateAssetMenu(fileName = "FoliageObject_", menuName = "ScriptableObjects/FoliageObject_", order = 1)]
public class FoliageObect : ScriptableObject
{
    [SerializeField] private GameObject objectPrefab = null;
    [SerializeField] private Mesh mesh = null;
    [SerializeField] private Material material = null;
    [SerializeField] private float minimalDistance = 2f;


    public GameObject ObjectPrefab { get => objectPrefab; }
    public Mesh Mesh { get => mesh; }
    public Material Material { get => material; }
    public float MinimalDistance { get => minimalDistance; }
}