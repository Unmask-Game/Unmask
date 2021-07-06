using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoliceMovement : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;

    // Bottom of the player object
    [SerializeField] private Transform groundCheck;

    // Map/Environment the player is walking on
    [SerializeField] private LayerMask groundMask;

    private float playerSpeed;

    private const float GroundDistance = 0f;
    private const float Gravity = -10;
    private float _verticalVelocity;
    public bool isWalking;

    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _startPosition;

    private PhotonView _view;

    private void Awake()
    {
        _startPosition = transform.position;
        _view = GetComponent<PhotonView>();
        playerSpeed = Constants.DesktopWalkSpeed;
    }

    private void Update()
    {
        if (_view.IsMine)
        {
            // Check if player is walking and sync outcome with others
            var walking = playerController.velocity.magnitude > 0;
            if (walking != isWalking)
            {
                isWalking = walking;
                _view.RPC("SetIsWalking", RpcTarget.Others, isWalking);
            }
        }

        Movement();
    }

    [PunRPC]
    private void SetIsWalking(bool isWalking)
    {
        this.isWalking = isWalking;
    }

    public void PlayerMove(InputAction.CallbackContext context)
    {
        // Get horizontal and vertical controller / keyboard input and set move direction
        var input = Vector3.ClampMagnitude(context.ReadValue<Vector2>(), 1);
        _moveDirection = new Vector3(input.x, 0, input.y);
    }

    private void Movement()
    {
        // In case the player ever falls out of bounds, he will be reset to his spawning point
        if (transform.position.y < -50)
        {
            transform.position = _startPosition;
        }
        else
        {
            if (Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask))
            {
                // No vertical velocity / gravity when standing on ground
                _verticalVelocity = 0;
            }
            
            // Apply gravity
            _verticalVelocity += Gravity * Time.deltaTime;
            playerController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);

            // Sprint when holding Shift 
            if (Keyboard.current.shiftKey.wasPressedThisFrame)
            {
                playerSpeed = Constants.DesktopSprintSpeed;
            }
            else if (Keyboard.current.shiftKey.wasReleasedThisFrame)
            {
                playerSpeed = Constants.DesktopWalkSpeed;
            }

            // Move player in set direction and apply rotation
            playerController.Move(playerSpeed * Time.deltaTime * (transform.rotation * _moveDirection));
        }
    }
}