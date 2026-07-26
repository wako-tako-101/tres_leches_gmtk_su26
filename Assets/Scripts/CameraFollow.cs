using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneWaypoint
    {
        public Transform position;
        public Transform lookTarget;
        public float moveDuration = 2f;
        public float pauseDuration = 0.5f;
    }

    [Header("Player Follow")]
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Cutscene")]
    [SerializeField] private CutsceneWaypoint[] waypoints;
    [SerializeField] private float returnDuration = 2f;
    [SerializeField] private bool playCutsceneOnStart = false;

    [Header("Player Control")]
    [SerializeField] private MonoBehaviour playerMovementScript;

    private float fixedZ;
    private bool isCutscenePlaying = false;

    private void Start()
    {
        fixedZ = transform.position.z;

        if (playCutsceneOnStart)
        {
            StartCoroutine(PlayCutscene());
        }
    }

    private void LateUpdate()
    {
        if (isCutscenePlaying)
            return;

        if (player == null)
            return;

        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            fixedZ
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void StartCutscene()
    {
        if (!isCutscenePlaying)
        {
            StartCoroutine(PlayCutscene());
        }
    }

    private IEnumerator PlayCutscene()
    {
        isCutscenePlaying = true;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        foreach (CutsceneWaypoint waypoint in waypoints)
        {
            if (waypoint.position == null)
                continue;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = new Vector3(
                waypoint.position.position.x,
                waypoint.position.position.y,
                fixedZ
            );

            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = startRotation;

            if (waypoint.lookTarget != null)
            {
                Vector3 direction = waypoint.lookTarget.position - targetPosition;

                if (direction != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(
                        Vector3.forward,
                        direction
                    );
                }
            }

            float elapsedTime = 0f;

            while (elapsedTime < waypoint.moveDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp01(
                    elapsedTime / waypoint.moveDuration
                );

                t = Mathf.SmoothStep(0f, 1f, t);

                transform.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

                transform.rotation = Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

                yield return null;
            }

            transform.position = targetPosition;
            transform.rotation = targetRotation;

            if (waypoint.pauseDuration > 0f)
            {
                yield return new WaitForSeconds(
                    waypoint.pauseDuration
                );
            }
        }

        if (player != null)
        {
            Vector3 startPosition = transform.position;

            Vector3 targetPosition = new Vector3(
                player.position.x,
                player.position.y,
                fixedZ
            );

            float elapsedTime = 0f;

            while (elapsedTime < returnDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = Mathf.Clamp01(
                    elapsedTime / returnDuration
                );

                t = Mathf.SmoothStep(0f, 1f, t);

                transform.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

                yield return null;
            }

            transform.position = targetPosition;
        }

        transform.rotation = Quaternion.identity;

        isCutscenePlaying = false;

        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
    }
}