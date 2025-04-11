using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int gridSize = 7;
    public float spacing = 1.0f;
    public GameObject dotPrefab;
    public Transform gridParent;
    public DotColor[] dotColors;

    private Dot[,] grid;

    public Dot[,] Grid => grid;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        InitGrid();
    }

    void InitGrid()
    {
        grid = new Dot[gridSize, gridSize];

        float offset = (gridSize - 1) * spacing * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                CreateDot(x, y, offset);
            }
        }
    }

    void CreateDot(int x, int y, float offset)
    {
        GameObject dotObj = Instantiate(dotPrefab, gridParent);
        dotObj.transform.localPosition = new Vector3(x * spacing - offset, y * spacing - offset, 0f);

        Dot dot = dotObj.GetComponent<Dot>();
        dot.Init(x, y, GetRandomColor());
        grid[x, y] = dot;
    }

    DotColor GetRandomColor()
    {
        return dotColors[Random.Range(0, dotColors.Length)];
    }

    public void RefillGrid()
    {
        StartCoroutine(RefillCoroutine());
    }

    private IEnumerator RefillCoroutine()
    {
        for (int x = 0; x < gridSize; x++)
        {
            int newY = 0;

            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] != null)
                {
                    if (newY != y)
                    {
                        grid[x, newY] = grid[x, y];
                        grid[x, y] = null;

                        StartCoroutine(MoveToPosition(grid[x, newY].transform, GetWorldPosition(x, newY), 0.2f));
                        grid[x, newY].Init(x, newY, grid[x, newY].color);
                    }
                    newY++;
                }
            }

            for (int y = newY; y < gridSize; y++)
            {
                Dot newDot = CreateDot(x, y, spawnAbove: true);
                grid[x, y] = newDot;

                Vector3 targetPos = GetWorldPosition(x, y);
                StartCoroutine(MoveToPosition(newDot.transform, targetPos, 0.25f));
                newDot.Init(x, y, newDot.color);

                yield return new WaitForSeconds(0.02f); 
            }
        }
    }


    public Dot CreateDot(int x, int y, bool spawnAbove = false)
    {
        Vector3 spawnPos = spawnAbove ? GetWorldPosition(x, gridSize) : GetWorldPosition(x, y);
        GameObject dotObject = Instantiate(dotPrefab, spawnPos, Quaternion.identity, transform);
        Dot dot = dotObject.GetComponent<Dot>();
        DotColor randomColor = (DotColor)Random.Range(0, System.Enum.GetValues(typeof(DotColor)).Length);
        dot.Init(x, y, randomColor);
        return dot;
    }


    public void RemoveDot(Dot dot)
    {
        if (dot == null) return;

        Vector2Int pos = dot.Position;
        Destroy(dot.gameObject);

        if (grid[pos.x, pos.y] == dot)
        {
            grid[pos.x, pos.y] = null;
        }
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(
            x * spacing - (gridSize - 1) * spacing * 0.5f,
            y * spacing - (gridSize - 1) * spacing * 0.5f,
            0f
        );
    }


    private IEnumerator MoveToPosition(Transform obj, Vector3 target, float duration)
    {
        if (obj == null) yield break;

        Vector3 start = obj.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (obj == null) yield break;

            obj.localPosition = Vector3.Lerp(start, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (obj != null)
            obj.localPosition = target;
    }



}
