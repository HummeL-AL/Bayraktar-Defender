using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellEventChannel cellCreated = null;
    [SerializeField] private CellEventChannel cellDestroyed = null;
    [SerializeField] private GameObject groupRenderer = null;

    public Dictionary<Material, List<FoliageInstance>> MeshGroups = new Dictionary<Material, List<FoliageInstance>>();
    public Vector2 halfBounds = Vector2.one;
    public Vector2 center = Vector2.zero;
    public Cell CellPrefab = null;

    private Dictionary<Material, MeshFilter> typeRenderers = new Dictionary<Material, MeshFilter>();
    private Cell[] cells = new Cell[4];
    private int lodLevel = 0;
    public bool isSeparated = false;

    public int LODLevel
    {
        get => lodLevel;
        set
        {
            lodLevel = value;
            UpdateRenderers();
        }
    }

    public void SetValues(Vector2 position, Vector2 halfSize)
    {
        MeshGroups = new Dictionary<Material, List<FoliageInstance>>();
        typeRenderers = new Dictionary<Material, MeshFilter>();
        cells = new Cell[4];
        isSeparated = false;

        center = position;
        halfBounds = halfSize;
    }

    public void AddObjects(FoliageInstance[] foliageInstance)
    {
        if (isSeparated)
        {
            RedirectObject(foliageInstance);
        }
        else
        {
            RecieveObject(foliageInstance);
        }
    }

    public void Separate()
    {
        isSeparated = true;
        Vector2 quarterBounds = halfBounds / 2f;
        
        cells[0] = CreateCell(center + new Vector2(-quarterBounds.x, quarterBounds.y), quarterBounds);
        cells[1] = CreateCell(center + new Vector2(quarterBounds.x, quarterBounds.y), quarterBounds);
        cells[2] = CreateCell(center + new Vector2(-quarterBounds.x, -quarterBounds.y), quarterBounds);
        cells[3] = CreateCell(center + new Vector2(quarterBounds.x, -quarterBounds.y), quarterBounds);

        foreach (KeyValuePair<Material, List<FoliageInstance>> objectType in MeshGroups)
        {
            RedirectObject(objectType.Value.ToArray());
        }

        foreach (Cell cell in cells)
        {
            cell.UpdateRenderers();
        }

        MeshGroups.Clear();
        ClearRenderers();
    }

    public void Merge()
    {
        if (isSeparated)
        {
            isSeparated = false;
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i].Merge();
                foreach (KeyValuePair<Material, List<FoliageInstance>> objectType in cells[i].MeshGroups)
                {
                    RecieveObject(objectType.Value.ToArray());
                }
                cells[i].Clear();

                UpdateRenderers();
            }
        }
    }

    public float GetDistance(Vector2 position)
    {
        float distance = 0f;

        bool inHorizontalBounds = position.x > center.x - halfBounds.x && position.x < center.x + halfBounds.x;
        bool inVerticalBounds = position.y > center.y - halfBounds.y && position.y < center.y + halfBounds.y;
        if (!(inHorizontalBounds && inVerticalBounds))
        {
            if(position.x < center.x && position.y >= center.y)
            {
                distance = Vector2.Distance(position, center + new Vector2(-halfBounds.x, halfBounds.y));
            }
            if (position.x > center.x && position.y >= center.y)
            {
                distance = Vector2.Distance(position, center + new Vector2(halfBounds.x, halfBounds.y));
            }
            if (position.x < center.x && position.y < center.y)
            {
                distance = Vector2.Distance(position, center + new Vector2(-halfBounds.x, -halfBounds.y));
            }
            if (position.x > center.x && position.y < center.y)
            {
                distance = Vector2.Distance(position, center + new Vector2(halfBounds.x, -halfBounds.y));
            }
        }

        return distance;
    }

    private void RecieveObject(FoliageInstance[] foliageInstances)
    {
        foreach (FoliageInstance foliageInstance in foliageInstances)
        {
            Material objectType = foliageInstance.ObjectData.Material;
            MeshGroups.TryGetValue(objectType, out List<FoliageInstance> meshesOfType);

            if (meshesOfType != null)
            {
                meshesOfType.Add(foliageInstance);
            }
            else
            {
                MeshGroups.Add(objectType, new List<FoliageInstance> { foliageInstance });
            }
        }
    }

    private void RedirectObject(FoliageInstance[] foliageInstances)
    {
        List<FoliageInstance>[] instanceGroups = new List<FoliageInstance>[4];
        for(int i = 0; i < instanceGroups.Length; i++)
        {
            instanceGroups[i] = new List<FoliageInstance>();
        }

        foreach (FoliageInstance foliageInstance in foliageInstances)
        {
            Vector2 position = foliageInstance.Position2D;
            int cellIndex = -1;

            if (position.x < center.x && position.y >= center.y)
            {
                cellIndex = 0;
            }
            if (position.x >= center.x && position.y >= center.y)
            {
                cellIndex = 1;
            }
            if (position.x < center.x && position.y < center.y)
            {
                cellIndex = 2;
            }
            if (position.x >= center.x && position.y < center.y)
            {
                cellIndex = 3;
            }

            instanceGroups[cellIndex].Add(foliageInstance);
        }

        for (int i = 0; i < instanceGroups.Length; i++)
        {
            cells[i].AddObjects(instanceGroups[i].ToArray());
        }
    }

    private void UpdateRenderers()
    {
        foreach (KeyValuePair<Material, List<FoliageInstance>> objectsType in MeshGroups)
        {
            UpdateRenderer(objectsType.Key);
        }
    }

    private void UpdateRenderer(Material objectType)
    {
        if (!isSeparated)
        {
            MeshGroups.TryGetValue(objectType, out List<FoliageInstance> objectsOfType);

            List<Mesh> meshesToCombine = new List<Mesh>();
            List<Matrix4x4> meshesTransforms = new List<Matrix4x4>();

            foreach (FoliageInstance fieldInstance in objectsOfType)
            {
                meshesToCombine.Add(fieldInstance.ObjectData.LODs[LODLevel]);
                meshesTransforms.Add(Matrix4x4.TRS(fieldInstance.Position, fieldInstance.Rotation, Vector3.one));
            }

            MeshCombiner meshCombiner = new MeshCombiner();
            Mesh combinedMesh = meshCombiner.CombineMeshes(meshesToCombine.ToArray(), meshesTransforms.ToArray());

            typeRenderers.TryGetValue(objectType, out MeshFilter filter);
            if (filter == null)
            {
                GameObject typeRenderer = Instantiate(groupRenderer, transform);
                typeRenderer.GetComponent<MeshRenderer>().sharedMaterial = objectType;
                filter = typeRenderer.GetComponent<MeshFilter>();

                typeRenderers.Add(objectType, filter);
            }

            filter.sharedMesh = combinedMesh;
        }
    }

    private Cell CreateCell(Vector2 position, Vector2 halfSize)
    {
        Cell cell = Instantiate(CellPrefab, transform);
        cell.SetValues(new Vector2(position.x, position.y), halfSize);
        cell.CellPrefab = CellPrefab;
        cell.LODLevel = LODLevel;

        cellCreated.RaiseEvent(cell);
        return cell;
    }

    private void Clear()
    {
        cellDestroyed.RaiseEvent(this);
        Destroy(gameObject);
    }

    private void ClearRenderers()
    {
        foreach (KeyValuePair<Material, MeshFilter> typeRenderer in typeRenderers)
        {
            Destroy(typeRenderer.Value.gameObject);
        }
        typeRenderers.Clear();
    }
}
