using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Foliage : MonoBehaviour
{
    [SerializeField] private CellEventChannel cellCreated = null;
    [SerializeField] private CellEventChannel cellDestroyed = null;

    [SerializeField] private Vector2 sizeLimits = Vector2.one;
    [SerializeField] private float[] LODDistances = new float[0];
    [SerializeField] private float maximalDistance = 25f;

    [SerializeField] private Cell cellPrefab = null;
    [SerializeField] private Vector2 fieldSize = Vector2.zero;

    private FoliageInstance[] foliageInstances = new FoliageInstance[0];
    private List<Cell> cells = new List<Cell>();

    [Inject] private Level level = null;
    [Inject] private Player player = null;

    private void Awake()
    {
        cellCreated.ChannelEvent += OnCellCreated;
        cellDestroyed.ChannelEvent += OnCellDestroyed;

        foliageInstances = level.LevelGraphics.FoliageInstances;
    }

    private void Start()
    {
        cells.Add(Instantiate(cellPrefab, transform));
        cells[0].center = new Vector2(transform.position.x, transform.position.z);
        cells[0].halfBounds = fieldSize / 2f;
        cells[0].CellPrefab = cellPrefab;

        InitializeField();
    }

    private void Update()
    {
        CheckDistances();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            Cell cell = cells[i];

            Vector3 position = new Vector3(cell.center.x, 1f, cell.center.y);
            Vector3 size = new Vector3(cell.halfBounds.x * 2f, 1f, cell.halfBounds.y * 2f);
            Gizmos.DrawWireCube(position, size);
        }
    }

    private void CheckDistances()
    {
        Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);
        for (int i = 0; i < cells.Count; i++)
        {
            Cell cell = cells[i];
            float distance = cell.GetDistance(playerPosition);

            if (cell.isSeparated)
            {
                if (distance >= maximalDistance && cell.halfBounds.x <= sizeLimits.y / 2f)
                {
                    cell.Merge();
                }
            }
            else if (distance < maximalDistance && cell.halfBounds.x >= sizeLimits.x / 2f)
            {
                cell.Separate();
            }

            int LODLevel = LODDistances.Length - 1;
            for (int j = 0; j < LODDistances.Length; j++)
            {
                if (distance < LODDistances[j])
                {
                    LODLevel = j;
                    break;
                }
            }

            if (cell.LODLevel != LODLevel)
            {
                cell.LODLevel = LODLevel;
            }
        }
    }

    private void InitializeField()
    {
        cells[0].AddObjects(foliageInstances);
    }

    private void OnCellCreated(Cell cell)
    {
        cells.Add(cell);
    }

    private void OnCellDestroyed(Cell cell)
    {
        cells.Remove(cell);
    }
}
