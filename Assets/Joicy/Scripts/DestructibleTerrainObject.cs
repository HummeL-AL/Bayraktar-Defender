using UnityEngine;

public class DestructibleTerrainObject : MonoBehaviour, IDamageable
{
    private Terrain parentTerrain = null;
    private int treeIndex = 0;

    public void SetTree(Terrain terrain, int index)
    {
        parentTerrain = terrain;
        treeIndex = index;
        Initialize();
    }

    public void TakeDamage(int damage, int damageLevel)
    {
        IDamageable[] damageables = SeparateTree().GetComponents<IDamageable>();
        for (int i = 0; i < damageables.Length; i++)
        {
            damageables[i].TakeDamage(damage, damageLevel);
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage, int damageLevel, float damageRadius)
    {
        IDamageable[] damageables = SeparateTree().GetComponents<IDamageable>();
        for (int i = 0; i < damageables.Length; i++)
        {
            damageables[i].TakeDamage(damage, damageLevel, damageRadius);
        }

        Destroy(gameObject);
    }

    private void Initialize()
    {
        CapsuleCollider reflectionCollider = GetComponent<CapsuleCollider>();
        CapsuleCollider originalCollider = GetComponent<CapsuleCollider>();

        reflectionCollider.height = originalCollider.height;
        reflectionCollider.radius = originalCollider.radius;
        reflectionCollider.center = originalCollider.center;
    }

    private GameObject SeparateTree()
    {
        TerrainData terrainData = parentTerrain.terrainData;
        TreeInstance originalTree = terrainData.treeInstances[treeIndex];
        GameObject separatedTree = Instantiate(terrainData.treePrototypes[treeIndex].prefab, originalTree.position, Quaternion.Euler(0f, originalTree.rotation, 0f), null);
        terrainData.treeInstances[treeIndex] = new TreeInstance();

        return separatedTree;
    }
}
