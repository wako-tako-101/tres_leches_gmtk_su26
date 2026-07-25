using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{
    [System.Serializable]
    public class SequenceStep
    {
        [Header("Event")]
        public UnityEvent onEvent;

        [Header("Delay After Event")]
        public float delayAfterEvent = 0f;
    }

    [Header("Sequence")]
    public List<SequenceStep> sequenceSteps = new List<SequenceStep>();

    private bool sequenceRunning = false;

    public void PlaySequence()
    {
        if (sequenceRunning)
            return;

        StartCoroutine(RunSequence());
    }
    public void ResetSequence()
    {
        StopAllCoroutines();

        sequenceRunning = false;
    }
    private IEnumerator RunSequence()
    {
        sequenceRunning = true;

        foreach (SequenceStep step in sequenceSteps)
        {
            // Invoke this step's events
            step.onEvent.Invoke();

            // Wait before moving to the next step
            if (step.delayAfterEvent > 0f)
            {
                yield return new WaitForSeconds(step.delayAfterEvent);
            }
        }

        sequenceRunning = false;
    }
}