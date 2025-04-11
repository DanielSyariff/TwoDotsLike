using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 8;
    public GameObject dotPrefab;
    public Transform dotParent;

    private Dot[,] grid;

    private void Start()
    {
        grid = new Dot[width, height];
        //GenerateInitialGrid();
    }

    void GenerateInitialGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnDot(x, y);
            }
        }
    }

    void SpawnDot(int x, int y)
    {
        Vector3 pos = new Vector3(x, y, 0);
        GameObject obj = Instantiate(dotPrefab, pos, Quaternion.identity, dotParent);
        Dot dot = obj.GetComponent<Dot>();
        DotColor color = (DotColor)Random.Range(0, System.Enum.GetValues(typeof(DotColor)).Length);
        dot.Init(x, y, color);
        grid[x, y] = dot;
    }

    public void Refill()
    {
        StartCoroutine(RefillRoutine());
    }

    IEnumerator RefillRoutine()
    {
        for (int x = 0; x < width; x++)
        {
            int emptyCount = 0;

            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    // Geser ke bawah
                    grid[x, y - emptyCount] = grid[x, y];
                    grid[x, y] = null;

                    Vector3 newPos = new Vector3(x, y - emptyCount, 0);
                    grid[x, y - emptyCount].transform.position = newPos;
                }
            }

            // Spawn Dot baru dari atas
            for (int i = 0; i < emptyCount; i++)
            {
                int y = height - emptyCount + i;
                Vector3 pos = new Vector3(x, y, 0);
                GameObject obj = Instantiate(dotPrefab, pos, Quaternion.identity, dotParent);
                Dot dot = obj.GetComponent<Dot>();
                DotColor color = (DotColor)Random.Range(0, System.Enum.GetValues(typeof(DotColor)).Length);
                dot.Init(x, height - i - 1, color);
                grid[x, height - i - 1] = dot;

                // Animasi jatuh
                Vector3 targetPos = new Vector3(x, height - i - 1, 0);
                StartCoroutine(AnimateDrop(dot.transform, targetPos));
            }
        }

        yield return null;
    }

    IEnumerator AnimateDrop(Transform obj, Vector3 target)
    {
        float t = 0f;
        Vector3 start = obj.position;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            obj.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        obj.position = target;
    }
}
