using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FoliageGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 fieldSize = Vector2.zero;
    [SerializeField] private FoliageObject[] avalaibleObjects = null;
    [SerializeField] private float[] weights = null; 
    [SerializeField] private int triesCount = 10;
    [SerializeField] private AnimationCurve distribution = null;
    [SerializeField] private LayerMask raycastLayerMask = 0;

    private List<int>[,] grid = null;
    private Vector2 halfSize = Vector2.zero;
    private float minimalRange = Mathf.Infinity;

    private List<FoliageInstance> generatedObjects = new List<FoliageInstance>();
    private List<FoliageInstance> spawnPoints = new List<FoliageInstance>();

    [Inject] private Level level = null;

    public void GenerateFoliage()
    {
        GetMinimalDistance();
        GetSpawnPoints();
        level.LevelGraphics.FoliageInstances = GetFilteredObjects().ToArray();

        ClearSpawnInfo();
    }

    private void Awake()
    {
        halfSize = fieldSize / 2f;
    }

    private void ClearSpawnInfo()
    {
        generatedObjects.Clear();
        spawnPoints.Clear();
        grid = new List<int>[0, 0];
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

        FoliageInstance instance = new FoliageInstance();
        instance.ObjectData = avalaibleObjects[0];
        instance.Position2D = fieldSize / 2f;
        spawnPoints.Add(instance);

        while (spawnPoints.Count > 0)
        {
            for (int i = 0; i < triesCount; i++)
            {
                int objectType = GetRandomObject();
                FoliageInstance candidate = new FoliageInstance();
                candidate.ObjectData = avalaibleObjects[objectType];

                float rotation = Random.value * Mathf.PI * 2f;
                Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
                float distance = Mathf.Max(spawnPoints[0].ObjectData.MinimalDistance, candidate.ObjectData.MinimalDistance);
                float offset = Random.Range(distance, distance + minimalRange);

                Vector2 raycastPosition = spawnPoints[0].Position2D + direction * offset;
                Physics.Raycast(new Vector3(raycastPosition.x, 100f, raycastPosition.y), Vector3.down, out RaycastHit hit, 200f, raycastLayerMask);
                candidate.Position = hit.point;

                candidate.Rotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));

                if (IsValid(candidate, cellSize))
                {
                    generatedObjects.Add(candidate);
                    spawnPoints.Add(candidate);

                    i = 0;
                }
            }
            spawnPoints.RemoveAt(0);
        }
    }

    private List<FoliageInstance> GetFilteredObjects()
    {
        List<FoliageInstance> filtered = new List<FoliageInstance>();
        foreach (FoliageInstance generatedObject in generatedObjects)
        {
            if (IsLucky(generatedObject.Position.y))
            {
                filtered.Add(generatedObject);
            }
        }

        return filtered;
    }

    private bool IsValid(FoliageInstance candidate, float cellSize)
    {
        Vector2 position = candidate.Position2D;
        if (position.x > -halfSize.x && position.x < halfSize.x && position.y > -halfSize.y && position.y < halfSize.y)
        {
            Vector2Int gridCoordinates = new Vector2Int((int)((position.x + halfSize.x) / cellSize), (int)((position.y + halfSize.y) / cellSize));

            int offset = Mathf.CeilToInt(candidate.ObjectData.MinimalDistance / cellSize);
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
                                float minimalDistance = Mathf.Max(generatedObjects[pointIndex].ObjectData.MinimalDistance, candidate.ObjectData.MinimalDistance);

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
            return true;
        }

        return false;
    }

    private bool IsLucky(float height)
    {
        float chanceToStay = distribution.Evaluate(height);
        return Random.value <= chanceToStay;
    }

    private void GetMinimalDistance()
    {
        minimalRange = Mathf.Infinity;

        foreach(FoliageObject foliageObect in avalaibleObjects)
        {
            minimalRange = Mathf.Min(minimalRange, foliageObect.MinimalDistance);
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