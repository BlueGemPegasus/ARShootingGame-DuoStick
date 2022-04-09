using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;

    // This two Value is to ensure player stick to the ground. 
    // Due to there is no Rigidbody used, manually adding gravity is needed
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        controller = GetComponent<CharacterController>();

    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.Player.Movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
}
