using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    [SerializeField] private PlayerInput playerInput;

    // Player Movement Main
    [SerializeField] private CharacterController controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 5.0f;
    [SerializeField] private float cDZ = 0.1f; // Controller's DeadZone

    // This two Value is to ensure player stick to the ground. 
    // Due to there is no Rigidbody used, manually adding gravity is needed
    private bool groundedPlayer;
    [SerializeField] private float gravityValue = -9.81f;

    // Player Aim and Shoot
    public GameObject bulletPrefab;
    public Transform firePoint;
    LineRenderer AimLine;

    // Player's Information
    public Text text_Name;
    public Text text_KillCount;
    public int score = 0;
    public ulong clientId;


    private void Awake()
    {
        playerInput = new PlayerInput();
        

        //GetComponent<Renderer>().material.color = playerColor;
        //text_Name.text = playerName;

    }

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            controller = GetComponent<CharacterController>();
            AimLine = GetComponent<LineRenderer>();
            AimLine.enabled = false;
            text_KillCount = GameObject.Find("txt_KillCount").GetComponent<Text>();
        }
    }

    private void OnEnable()
    {
        playerInput.Player.Aim.canceled += ShootServerRPC;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        if (IsLocalPlayer)
        {
            clientId = NetworkManager.Singleton.LocalClientId;
            Movement();
            Aiming();
            AimLine.enabled = true;
            Physics.Raycast(firePoint.position, firePoint.forward * 50, out RaycastHit AimHit, 50f);
            AimLine.SetPosition(0, firePoint.position);
            if(AimHit.collider)
            {
                AimLine.SetPosition(1, AimHit.point);
            }

            text_KillCount.text = "Kill Count : " + score.ToString(); 

            if (score == 10)
            {
                // End of Game
            }
        }
        //Debug.DrawRay(firePoint.position, transform.forward * 50, Color.blue) ;
    }

    void Movement()
    {
        if (IsLocalPlayer)
        {
            transform.position = Position.Value;
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            // Get the Vector2 from Left Joystick, and pass it to the player function. 
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

    void Aiming()
    {
        if (IsLocalPlayer)
        {
            Vector2 RightJoystick = playerInput.Player.Aim.ReadValue<Vector2>();
            if (Mathf.Abs(RightJoystick.x) > cDZ || Mathf.Abs(RightJoystick.y) > cDZ)
            {
                Vector3 aim = Vector3.right * RightJoystick.x + Vector3.forward * RightJoystick.y;
                if (aim.sqrMagnitude > 0.0f)
                {
                    Quaternion newrotation = Quaternion.LookRotation(aim, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, 1000f * Time.deltaTime * rotateSpeed);
                }
            }
        }
    }

    //public void Shooting(InputAction.CallbackContext obj)
    //{
    //    if (IsLocalPlayer)
    //    {
    //        GameObject BulletShoot = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    //        BulletShoot.GetComponent<NetworkObject>().Spawn();
    //        // Please take note that, this only allow THE SERVER spawn the object. Which is not suitable for this game.
    //    }

    //}

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRPC(InputAction.CallbackContext obj)
    {
        ShootClientRPC();
    }

    [ClientRpc]
    private void ShootClientRPC()
    {       
        var BulletShoot = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletShoot.GetComponent<Bullet>().owner = this;
        if (Physics.Raycast(firePoint.position, firePoint.forward * 50, out RaycastHit AimHit, 50f))
        {
            BulletShoot.transform.position = AimHit.point;
        }
    }

}
