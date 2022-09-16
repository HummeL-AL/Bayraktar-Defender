using System.Collections.Generic;
using UnityEngine;

public class Foliage : MonoBehaviour, IDamageable
{
    [SerializeField] private Level levelData = null;

    [SerializeField] private Vector2 fieldSize = Vector2.one;
    [SerializeField] private FoliageObect[] avalaibleObjects = null;
    [SerializeField] private float[] weights = null; 
    [SerializeField] private int triesCount = 10;
    [SerializeField] private LayerMask raycastLayerMask = 0;

    private Vector2 halfFieldSize = Vector2.one;

    private List<int>[,] grid = null;
    private float minimalRange = Mathf.Infinity;

    private List<GeneratedObject> generatedObjects = new List<GeneratedObject>();
    private List<GeneratedObject> spawnPoints = new List<GeneratedObject>();

    public void GenerateFoliage()
    {
        halfFieldSize = fieldSize / 2f;

        Clear();
        GetMinimalDistance();
        GetSpawnPoints();
        SortObjects();
    }

    public void Clear()
    {
        generatedObjects.Clear();
        spawnPoints.Clear();
        levelData.LevelGraphics.ObjectsTypes.Clear();
        grid = new List<int>[0, 0];

        int childs = transform.childCount;
        for(int i = 0; i < childs; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void TakeDamage(int damage, int damageLevel)
    {
        
    }

    public void TakeDamage(int damage, int damageLevel, float explosionRadius)
    {

    }

    private void GetSpawnPoints()
    {   
        float cellSize = minimalRange / Mathf.Sqrt(2f);
        int gridWidth = Mathf.CeilToInt(fieldSize.x / cellSize);
        int gridHeight = Mathf.CeilToInt(fieldSize.y / cellSize);

        grid = new List<int>[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                grid[i, j] = new List<int> { -1 };
            }
        }

        spawnPoints.Add(new GeneratedObject(avalaibleObjects[0]));
        spawnPoints[0].SetPosition(fieldSize / 2f);

        while (spawnPoints.Count > 0)
        {
            for (int i = 0; i < triesCount; i++)
            {
                int objectType = GetRandomObject();
                GeneratedObject candidate = new GeneratedObject(avalaibleObjects[objectType]);

                float rotation = Random.value * Mathf.PI * 2f;
                Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
                float distance = Mathf.Max(spawnPoints[0].Object.MinimalDistance, candidate.Object.MinimalDistance);
                float offset = Random.Range(distance, distance + minimalRange);

                candidate.SetPosition(spawnPoints[0].Position2D + direction * offset);
                Physics.Raycast(candidate.Position + Vector3.up * 100f, Vector3.down, out RaycastHit hit, 200f, raycastLayerMask);
                candidate.SetPosition(hit.point);

                candidate.SetRotation(Vector3.up * Random.Range(0f, 360f));

                if (IsValid(candidate, cellSize))
                {
                    i = 0;
                }
            }
            spawnPoints.RemoveAt(0);
        }
    }

    private bool IsValid(GeneratedObject candidate, float cellSize)
    {
        Vector2 position = candidate.Position2D;
        if (position.x > -halfFieldSize.x && position.x < halfFieldSize.x && position.y > -halfFieldSize.y && position.y < halfFieldSize.y)
        {
            Vector2Int gridCoordinates = new Vector2Int((int)((position.x + halfFieldSize.x) / cellSize), (int)((position.y + halfFieldSize.y) / cellSize));

            int offset = Mathf.CeilToInt(candidate.Object.MinimalDistance / cellSize);
            int minX = Mathf.Max(0, gridCoordinates.x - offset);
            int maxX = Mathf.Min(gridCoordinates.x + offset, grid.GetLength(0));
            int minY = Mathf.Max(0, gridCoordinates.y - offset);
            int maxY = Mathf.Min(gridCoordinates.y + offset, grid.GetLength(1));

            if (generatedObjects.Count > 0)
            {
                for (int i = minX; i < maxX; i++)
                {
                    for (int j = minY; j < maxY; j++)
                    {
                        List<int> pointIndexes = grid[i, j];
                        for (int k = 0; k < pointIndexes.Count; k++)
                        {
                            int pointIndex = pointIndexes[k];
                            if (pointIndex >= 0)
                            {
                                float distance = (position - generatedObjects[pointIndex].Position2D).magnitude;
                                float minimalDistance = Mathf.Max(generatedObjects[pointIndex].Object.MinimalDistance, candidate.Object.MinimalDistance);

                                if (distance < minimalDistance)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    grid[i, j].Add(generatedObjects.Count);
                }
            }

            generatedObjects.Add(candidate);
            spawnPoints.Add(candidate);
            return true;
        }

        return false;
    }

    private void GetMinimalDistance()
    {
        minimalRange = Mathf.Infinity;

        foreach(FoliageObect foliageObect in avalaibleObjects)
        {
            minimalRange = Mathf.Min(minimalRange, foliageObect.MinimalDistance);
        }
    }

    private void SortObjects()
    {
        List<Matrix4x4>[] matrices = new List<Matrix4x4>[avalaibleObjects.Length];
        for(int i = 0; i < matrices.Length; i++)
        {
            matrices[i] = new List<Matrix4x4>();
        }

        for (int i = 0; i < generatedObjects.Count; i++)
        {
            GeneratedObject generatedObject = generatedObjects[i];
            int objectType = System.Array.IndexOf(avalaibleObjects, generatedObject.Object);

            Matrix4x4 matrix = Matrix4x4.TRS(generatedObject.Position, generatedObject.Rotation, Vector3.one);
            matrices[objectType].Add(matrix);
        }

        levelData.LevelGraphics.ObjectsTypes = new List<GeneratedObjectType>();
        for (int i = 0; i < avalaibleObjects.Length; i++)
        {
            List<GeneratedObjectType> objectTypes = levelData.LevelGraphics.ObjectsTypes;
            FoliageObect objectType = avalaibleObjects[i];

            objectTypes.Add(new GeneratedObjectType());
            objectTypes[i].SetValues(objectType.Mesh, objectType.Material, matrices[i].ToArray());
        }

    }

    private int GetRandomObject()
    {
        float value = Random.value;

        float overallWeight = 0f;
        foreach(float weight in weights)
        {
            overallWeight += weight;
        }

        float accumulatedWeight = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            accumulatedWeight += weights[i] / overallWeight;
            if(value < accumulatedWeight)
            {
                return i;
            }
        }

        return 0;
    }
}

[System.Serializable]
public class GeneratedObject
{
    [SerializeField] public FoliageObect Object { get; private set; }
    [SerializeField] public Vector3 Position { get; private set; }
    [SerializeField] public Quaternion Rotation { get; private set; }

    [SerializeField] public Vector2 Position2D { get => new Vector2(Position.x, Position.z); }


    public GeneratedObject(FoliageObect foliageObect)
    {
        Object = foliageObect;
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public void SetPosition(Vector2 position)
    {
        Position = new Vector3(position.x, 0f, position.y);
    }

    public void SetRotation(Quaternion rotation)
    {
        Rotation = rotation;
    }

    public void SetRotation(Vector3 rotation)
    {
        Rotation = Quaternion.Euler(rotation);
    }
}