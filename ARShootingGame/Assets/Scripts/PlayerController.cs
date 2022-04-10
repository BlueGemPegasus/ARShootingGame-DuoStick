using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    // Player Movement Main
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;

    // This two Value is to ensure player stick to the ground. 
    // Due to there is no Rigidbody used, manually adding gravity is needed
    private bool groundedPlayer;
    [SerializeField] private float gravityValue = -9.81f;

    // Player Aim and Shoot
    public GameObject bulletPrefab;
    public Transform firePoint;

    // Player's Information
    public Text text_Name;
    public Text text_KillCount;

    private void Awake()
    {
        playerInput = new PlayerInput();

        controller = GetComponent<CharacterController>();

        //GetComponent<Renderer>().material.color = playerColor;
        //text_Name.text = playerName;
        
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
