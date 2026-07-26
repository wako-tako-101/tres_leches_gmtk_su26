using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// A script by Michael O'Connell, extended by Benjamin Cohen

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEvent
    {
        public string eventName;
        public UnityEngine.Events.UnityEvent onEvent;
    }

    [System.Serializable]
    public class DialogueAudio
    {
        public string audioName;
        public AudioClip audioClip;
    }

    [Header("Skip Dialogue")]
    public KeyCode skipKey = KeyCode.B;
    public bool allowSkip = true;

    [Header("Dialogue Events")]
    public List<DialogueEvent> dialogueEvents = new List<DialogueEvent>();

    [Header("Dialogue Audio")]
    public List<DialogueAudio> dialogueAudio = new List<DialogueAudio>();

    DialogueManager manager;

    public TextAsset TextFileAsset;

    private Queue<string> dialogue = new Queue<string>();

    public float waitTime = 0.5f;
    private float nextTime = 0f;

    public bool singleUseDialogue = false;

    [HideInInspector]
    public bool hasBeenUsed = false;

    bool inArea = false;

    // Prevents end-of-dialogue events from firing more than once
    private bool dialogueFinished = false;


    private void Awake()
    {
        manager = FindObjectOfType<DialogueManager>();
    }


    private void Update()
    {
        // Skip dialogue
        if (allowSkip && inArea && Input.GetKeyDown(skipKey))
        {
            SkipDialogue();
            return;
        }

        // Advance dialogue
        if (!hasBeenUsed &&
            inArea &&
            Input.GetKeyDown(KeyCode.E) &&
            nextTime < Time.timeSinceLevelLoad)
        {
            nextTime = Time.timeSinceLevelLoad + waitTime;

            manager.AdvanceDialogue();
        }
    }


    // Called when you want to start dialogue
    public void TriggerDialogue()
    {
        // Reset the dialogue finished state
        dialogueFinished = false;

        ReadTextFile();

        manager.StartDialogue(dialogue);
    }


    // Loads in your text file
    private void ReadTextFile()
    {
        string txt = TextFileAsset.text;

        string[] lines = txt.Split(
            System.Environment.NewLine.ToCharArray()
        );

        SearchForTags(lines);

        // Add the end marker
        dialogue.Enqueue("EndQueue");
    }


    // Used by [EVENT=EventName] tags in the dialogue
    public void ExecuteEvent(string eventName)
    {
        foreach (DialogueEvent dialogueEvent in dialogueEvents)
        {
            if (dialogueEvent.eventName == eventName)
            {
                dialogueEvent.onEvent.Invoke();
                return;
            }
        }

        Debug.LogWarning("No dialogue event found with name: " + eventName);
    }


    // Called by DialogueManager when the dialogue has completely ended
    public void OnDialogueFinished()
    {
        // Prevent the events from firing multiple times
        if (dialogueFinished)
            return;

        dialogueFinished = true;

        Debug.Log("Dialogue finished for: " + gameObject.name);

        // Invoke all events assigned to this dialogue trigger
        foreach (DialogueEvent dialogueEvent in dialogueEvents)
        {
            if (dialogueEvent.onEvent != null)
            {
                dialogueEvent.onEvent.Invoke();
            }
        }
    }


    // Skip the rest of the dialogue
    public void SkipDialogue()
    {
        if (manager == null)
            return;

        dialogue.Clear();

        // Add the end marker
        dialogue.Enqueue("EndQueue");

        // Tell the DialogueManager to advance to the end
        manager.AdvanceDialogue();

        OnDialogueFinished();
    }


    public void ResetDialogueTrigger()
    {
        dialogue.Clear();

        hasBeenUsed = false;
        inArea = false;
        nextTime = 0f;

        // Allow end events to trigger again
        dialogueFinished = false;

        if (manager != null && manager.currentTrigger == this)
        {
            manager.currentTrigger = null;
        }
    }


    /*
     * Version 2:
     * Introduces the ability to have multiple tags on a single line.
     * Allows for multiple functions to be programmed to unique text strings
     * or general functions.
     */
    private void SearchForTags(string[] lines)
    {
        foreach (string line in lines)
        {
            // Ignore empty lines
            if (!string.IsNullOrEmpty(line))
            {
                // Check for a dialogue tag
                if (line.StartsWith("["))
                {
                    // Example:
                    // [NAME=Michael] Hello, my name is Michael

                    string special = line.Substring(
                        0,
                        line.IndexOf(']') + 1
                    );

                    string curr = line.Substring(
                        line.IndexOf(']') + 1
                    );

                    // Add the special tag
                    dialogue.Enqueue(special);

                    // Process the remaining text
                    string[] remainder =
                        curr.Split(System.Environment.NewLine.ToCharArray());

                    SearchForTags(remainder);
                }
                else
                {
                    // Normal dialogue line
                    dialogue.Enqueue(line);
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenUsed)
        {
            manager.currentTrigger = this;

            TriggerDialogue();
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inArea = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            manager.EndDialogue();
        }

        inArea = false;
    }
}
