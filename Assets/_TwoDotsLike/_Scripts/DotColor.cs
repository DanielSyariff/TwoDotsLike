using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DotColor
{
    Red, Blue, Green, Yellow, Purple
}


public class Dot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer spriteRenderer;
    public DotColor color;
    private bool isConnected = false;


    private int x, y;

    public Vector2Int Position => new Vector2Int(x, y);

    public void Init(int x, int y, DotColor color)
    {
        this.x = x;
        this.y = y;
        SetColor(color);
    }

    public void SetColor(DotColor newColor)
    {
        color = newColor;
        spriteRenderer.color = GetColorValue(color);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CycleColor();
    }

    void CycleColor()
    {
        int next = ((int)color + 1) % System.Enum.GetValues(typeof(DotColor)).Length;
        SetColor((DotColor)next);
    }

    Color GetColorValue(DotColor color)
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        DotConnector.Instance.BeginConnection(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            Dot other = hit.collider.GetComponent<Dot>();
            if (other != null)
            {
                DotConnector.Instance.TryAddDot(other);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DotConnector.Instance.EndConnection();
    }

    public void PlayConnectAnimation()
    {
        isConnected = true;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void ResetScale()
    {
        if (!isConnected)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void ForceReset()
    {
        isConnected = false;
        transform.localScale = Vector3.one;
    }

}
