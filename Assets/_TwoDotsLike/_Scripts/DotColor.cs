using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DotColor
{
    Red, Blue, Green, Yellow, Purple
}

public class Dot : MonoBehaviour, IPointerClickHandler
{
    public SpriteRenderer spriteRenderer;
    public DotColor color;

    private int x, y;

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
}