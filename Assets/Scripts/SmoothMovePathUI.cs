using System.Collections;
using UnityEngine;

public class SmoothMovePathUI : MonoBehaviour
{
    [Header("Object to Move")]
    public Transform objectToMove;

    [Header("Target Points")]
    public Transform[] targetPoints;

    [Header("Movement Settings")]
    public float moveDuration = 1f;

    private Vector2 startingAnchoredPosition;
    private Vector3 startingWorldPosition;

    private void Start()
    {
        if (objectToMove == null)
            return;

        RectTransform objectRect = objectToMove.GetComponent<RectTransform>();

        if (objectRect != null)
        {
            startingAnchoredPosition = objectRect.anchoredPosition;
        }
        else
        {
            startingWorldPosition = objectToMove.position;
        }
    }

    public void ResetMovement()
    {
        StopAllCoroutines();

        if (objectToMove == null)
            return;

        RectTransform objectRect = objectToMove.GetComponent<RectTransform>();

        if (objectRect != null)
        {
            objectRect.anchoredPosition = startingAnchoredPosition;
        }
        else
        {
            objectToMove.position = startingWorldPosition;
        }
    }

    public void MoveAlongPath()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (objectToMove == null || targetPoints.Length == 0)
            yield break;

        RectTransform objectRect = objectToMove.GetComponent<RectTransform>();

        foreach (Transform targetPoint in targetPoints)
        {
            RectTransform targetRect = targetPoint.GetComponent<RectTransform>();

            Vector3 startPosition;
            Vector3 targetPosition;

            // If the object is a UI element
            if (objectRect != null && targetRect != null)
            {
                startPosition = objectRect.anchoredPosition;
                targetPosition = targetRect.anchoredPosition;
            }
            // If the object is a normal world object
            else
            {
                startPosition = objectToMove.position;
                targetPosition = targetPoint.position;
            }

            float elapsedTime = 0f;

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / moveDuration;
                t = Mathf.SmoothStep(0f, 1f, t);

                if (objectRect != null && targetRect != null)
                {
                    objectRect.anchoredPosition = Vector3.Lerp(
                        startPosition,
                        targetPosition,
                        t
                    );
                }
                else
                {
                    objectToMove.position = Vector3.Lerp(
                        startPosition,
                        targetPosition,
                        t
                    );
                }

                yield return null;
            }

            // Ensure exact final position
            if (objectRect != null && targetRect != null)
            {
                objectRect.anchoredPosition = targetPosition;
            }
            else
            {
                objectToMove.position = targetPosition;
            }
        }
    }
}