using UnityEngine;

public class EquationChecker : MonoBehaviour
{
    public NumberSlot[] slots;

    public string[] correctOrder;

    public Sprite disabledButtonSprite;
    public Sprite activeButtonSprite;

    public SpriteRenderer buttonSpriteRenderer;

    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    public bool EquationReady { get; private set; }

    private void Start()
    {
        buttonSpriteRenderer.sprite = disabledButtonSprite;
        EquationReady = false;
    }

    private void Update()
    {
        CheckIfEquationReady();
    }

    private void CheckIfEquationReady()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentBox == null)
            {
                SetButtonInactive();
                return;
            }
        }
        Debug.Log("Button is active");
        SetButtonActive();
    }

    private void SetButtonInactive()
    {
        EquationReady = false;
        buttonSpriteRenderer.sprite = disabledButtonSprite;
    }

    private void SetButtonActive()
    {
        EquationReady = true;
        buttonSpriteRenderer.sprite = activeButtonSprite;
    }

    public void CheckEquation()
    {
        if (!EquationReady)
        {
            return;
        }

        if (slots.Length != correctOrder.Length)
        {
            Debug.LogError("Number of slots does not match the correct answer!");
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentBox == null)
            {
                PlayIncorrectSound();
                return;
            }

            if (!slots[i].currentBox.CompareTag(correctOrder[i]))
            {
                PlayIncorrectSound();
                return;
            }
        }

        PlayCorrectSound();
    }

    private void PlayCorrectSound()
    {
        Debug.Log("CORRECT EQUATION!");

        if (audioSource != null && correctSound != null)
        {
            audioSource.PlayOneShot(correctSound);
        }
    }

    private void PlayIncorrectSound()
    {
        Debug.Log("INCORRECT EQUATION!");

        if (audioSource != null && incorrectSound != null)
        {
            audioSource.PlayOneShot(incorrectSound);
        }
    }
}