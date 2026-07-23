using UnityEngine;

public class TopDownMovement3D : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody playerRigid;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);

        playerRigid.MovePosition(playerRigid.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
