using UnityEngine;

public class DisableGameplayObjects : MonoBehaviour
{
    public void DisableGameplay()
    {
        // Find and disable all objects tagged "Player"
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }

        // Find and disable all objects tagged "DialogueTrigger"
        GameObject[] dialogueTriggers = GameObject.FindGameObjectsWithTag("DialogueTrigger");

        foreach (GameObject dialogueTrigger in dialogueTriggers)
        {
            dialogueTrigger.SetActive(false);
        }
    }
}