using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isDisabled = false;

    private Rigidbody2D playerRigid;
    private Vector2 moveInput;

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDisabled)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            moveInput.y += 1;

        if (Keyboard.current.sKey.isPressed)
            moveInput.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            moveInput.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            moveInput.x += 1;

        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        if (isDisabled)
            return;

        playerRigid.MovePosition(
            playerRigid.position + moveInput * moveSpeed * Time.fixedDeltaTime
        );
    }
}