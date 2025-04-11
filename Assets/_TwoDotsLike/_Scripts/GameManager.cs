using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public int gridSize = 7;
    public GameObject dotPrefab;
    public Transform gridParent;
    public DotColor[] dotColors;

    private Dot[,] grid;

    void Start()
    {
        InitGrid();
    }

    void InitGrid()
    {
        grid = new Dot[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                CreateDot(x, y);
            }
        }
    }

    void CreateDot(int x, int y)
    {
        GameObject dotObj = Instantiate(dotPrefab, gridParent);
        dotObj.transform.localPosition = new Vector3(x, y, 0);

        Dot dot = dotObj.GetComponent<Dot>();
        dot.Init(x, y, GetRandomColor());
        grid[x, y] = dot;
    }

    DotColor GetRandomColor()
    {
        return dotColors[Random.Range(0, dotColors.Length)];
    }
}