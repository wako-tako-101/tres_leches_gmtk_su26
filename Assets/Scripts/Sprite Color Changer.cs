using UnityEngine;

public class SpriteColorChanger : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public Color targetColor = Color.white;

    public void SetColor()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            if (sprite != null)
            {
                sprite.color = targetColor;
            }
        }
    }
}