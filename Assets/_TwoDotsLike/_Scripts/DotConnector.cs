using System.Collections.Generic;
using UnityEngine;

public class DotConnector : MonoBehaviour
{
    public static DotConnector Instance;
    public LineRenderer lineRenderer;

    private List<Dot> connectedDots = new List<Dot>();
    private DotColor currentColor;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginConnection(Dot startDot)
    {
        connectedDots.Clear();
        connectedDots.Add(startDot);
        currentColor = startDot.color;
        startDot.PlayConnectAnimation();

        UpdateLineRenderer();
    }

    public void TryAddDot(Dot dot)
    {
        if (connectedDots.Contains(dot))
        {
            dot.ResetScale();
            return;
        }

        if (dot.color != currentColor)
        {
            dot.ResetScale();
            return;
        }

        Dot lastDot = connectedDots[connectedDots.Count - 1];
        if (IsAdjacent(lastDot, dot))
        {
            connectedDots.Add(dot);
            dot.PlayConnectAnimation();
            UpdateLineRenderer();
        }
        else
        {
            dot.ResetScale();
        }
    }



    public void EndConnection()
    {

        if (connectedDots.Count >= 3)
        {
            foreach (Dot dot in connectedDots)
            {
                StartCoroutine(AnimateAndDestroy(dot));
                GameManager.Instance.RemoveDot(dot);
                //Destroy(dot.gameObject);
            }

            GameManager.Instance.RefillGrid();

        }
        else
        {
            foreach (Dot dot in connectedDots)
            {
                dot.ForceReset();
            }
        }

        connectedDots.Clear();
        UpdateLineRenderer();
    }
    private System.Collections.IEnumerator AnimateAndDestroy(Dot dot)
    {
        float duration = 0.2f;
        float elapsed = 0f;

        Vector3 originalScale = dot.transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (elapsed < duration)
        {
            dot.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        dot.transform.localScale = targetScale;
        Destroy(dot.gameObject);
    }


    private bool IsAdjacent(Dot a, Dot b)
    {
        int dx = Mathf.Abs(a.Position.x - b.Position.x);
        int dy = Mathf.Abs(a.Position.y - b.Position.y);
        return (dx + dy) == 1;
    }

    private void Update()
    {
        if (connectedDots.Count > 0)
        {
            UpdateLineRenderer();
        }
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null)
        {
            return;
        }

        int count = connectedDots.Count;
        lineRenderer.positionCount = count + 1;

        for (int i = 0; i < count; i++)
        {
            lineRenderer.SetPosition(i, connectedDots[i].transform.position);
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        lineRenderer.SetPosition(count, mouseWorldPos);

        if (count > 0)
        {
            Color baseColor = GetColorValue(currentColor);
            lineRenderer.startColor = baseColor;
            lineRenderer.endColor = baseColor;
        }
    }

    private Color GetColorValue(DotColor color)
    {
        switch (color)
        {
            case DotColor.Red: return Color.red;
            case DotColor.Blue: return Color.blue;
            case DotColor.Green: return Color.green;
            case DotColor.Yellow: return Color.yellow;
            case DotColor.Purple: return new Color(0.5f, 0f, 0.5f);
            default: return Color.white;
        }
    }
}
