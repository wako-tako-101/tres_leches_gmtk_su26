using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Hover Events")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    [Header("Optional Color Change")]
    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;

    [Header("Optional Scale")]
    public bool scaleOnHover = true;
    public float hoverScale = 1.1f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Trigger events assigned in Inspector
        onHoverEnter.Invoke();

        // Change color
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }

        // Scale button
        if (scaleOnHover)
        {
            transform.localScale = originalScale * hoverScale;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Trigger events assigned in Inspector
        onHoverExit.Invoke();

        // Reset color
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }

        // Reset scale
        if (scaleOnHover)
        {
            transform.localScale = originalScale;
        }
    }
}