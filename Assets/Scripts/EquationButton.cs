using UnityEngine;

public class EquationButton : MonoBehaviour
{
    public EquationChecker equationChecker;

    public Animator animator;

    public string playerTag = "Player";

    private bool playerNearby;

    private void Update()
    {
        if (equationChecker.EquationReady)
        {
            if (playerNearby)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.enabled = true;
                    PressButton();
                }
            }
        } else
        {
            animator.enabled = false;
        }
    }

    private void PressButton()
    {
        animator.SetTrigger("Press");

        equationChecker.CheckEquation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerNearby = false;
        }
    }
}