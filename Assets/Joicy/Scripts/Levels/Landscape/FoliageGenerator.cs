using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FoliageGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] plantPrefabs = null;

    [SerializeField] private Vector2 mapSize = Vector2.one;
    [SerializeField] private Texture2D landscapeTexture = null;
    [SerializeField] private float landscapeHeight = 0f;
    [SerializeField] private AnimationCurve distanceBeetweenObjects = null;

    [SerializeField] private int cancelCount = 10;

    private Vector2 halfSize = Vector2.one;
    private Dictionary<Vector2, float> plantPositions = new Dictionary<Vector2, float>();


    public void GenerateFoliage()
    {
        Clear();

        for (int i = 0; i < cancelCount; i++)
        {
            Vector2 position = GetRandomPosition();
            float minimalDistance = GetMinimalDistance(position);
            bool spawnable = CheckDistance(position, minimalDistance);

            if(spawnable)
            {
                CreatePlant(position);
                i = 0;
            }
        }
    }

    public void Clear()
    {
        plantPositions.Clear();

        int childs = transform.childCount;
        for(int i = 0; i < childs; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    private void OnValidate()
    {
        halfSize = mapSize / 2f;
    }

    private Vector2 GetRandomPosition()
    {
        float xCoord = Random.Range(-halfSize.x, halfSize.y);
        float yCoord = Random.Range(-halfSize.x, halfSize.y);
        return new Vector2(xCoord, yCoord);
    }

    private Vector2 GetTexturePosition(Vector2 mapPosition)
    {
        float xPercent = Mathf.InverseLerp(-halfSize.x, halfSize.x, mapPosition.x);
        float yPercent = Mathf.InverseLerp(-halfSize.y, halfSize.y, mapPosition.y);

        int xCoord = (int)(xPercent * landscapeTexture.width);
        int yCoord = (int)(yPercent * landscapeTexture.height);
        return new Vector2(xCoord, yCoord);
    }

    private float GetMinimalDistance(Vector2 position)
    {
        float minimalDistance = 0f;

        if(plantPositions.TryGetValue(position, out minimalDistance))
        {
            return minimalDistance;
        }
        else
        {
            Vector2 texturePosition = GetTexturePosition(position);
            float percent = GetTextureValue(texturePosition);

            minimalDistance = distanceBeetweenObjects.Evaluate(percent);
            plantPositions.Add(position, minimalDistance);
        }

        return minimalDistance;
    }

    private float GetTextureValue(Vector2 coords)
    {
        return landscapeTexture.GetPixel((int)coords.x, (int)coords.y).r;
    }

    private bool CheckDistance(Vector2 position, float minimalDistance)
    {
        bool hasIntersections = false;
        foreach (KeyValuePair<Vector2, float> plantPosition in plantPositions)
        {
            if (plantPosition.Key != position)
            {
                float distance = Vector2.Distance(position, plantPosition.Key);
                float otherMinimalDistance = plantPosition.Value;

                if (distance < minimalDistance && distance < otherMinimalDistance)
                {
                    hasIntersections = true;
                    break;
                }
            }
        }
        return !hasIntersections;
    }

    private void CreatePlant(Vector2 position)
    {
        int plantIndex = Random.Range(0, plantPrefabs.Length);
        GameObject prefab = plantPrefabs[plantIndex];
        
        Vector2 texturePosition = GetTexturePosition(position);
        float height = GetTextureValue(texturePosition) * landscapeHeight;

        Vector3 spawnPos = new Vector3(position.x, height, position.y);
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        GameObject plant = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
        plant.transform.position = spawnPos;
        plant.transform.rotation = rotation;
    }
}
