using Pathfinding;
using UnityEngine;

public class VirusChase : MonoBehaviour
{
    private IAstarAI ai;
    private Transform player;

    [SerializeField] private float repathInterval = 0.2f;

    private float repathTimer;

    private void Awake()
    {
        ai = GetComponent<IAstarAI>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("VirusChase: No GameObject with the 'Player' tag was found.");
        }
    }

    private void Update()
    {
        if (player == null || ai == null)
            return;

        // Always keep the destination pointed at the player.
        ai.destination = player.position;

        // Periodically request a new path.
        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            ai.SearchPath();
            repathTimer = repathInterval;
        }
    }
}