using System;
using UnityEngine;

// TODO: VR-player support
// TODO: Should be able to get a temporary speed boost or temporary slow down
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    private ItemController _items;
    private float _originalSpeed;
    public float playerSpeed = 5;

    // Bottom of the player object
    [SerializeField] private Transform groundCheck;

    // Map/Environment the player is walking on
    [SerializeField] private LayerMask groundMask;

    private const float GroundDistance = 0f;
    private const float Gravity = -10;
    private float _verticalVelocity;

    private Vector3 _moveDirection;
    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
        _originalSpeed = playerSpeed;
    }

    private void Start()
    {
        _items = GetComponent<ItemController>();
    }

    private void Update()
    {
        GetKeyboardInput();
        Movement();
    }

    private void GetKeyboardInput()
    {
        _moveDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, 1);
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

            // Gravity
            _verticalVelocity += Gravity * Time.deltaTime;
            playerController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);

            playerSpeed = _items.IsAttacking() ? _originalSpeed / 3 : _originalSpeed;
            playerController.Move(playerSpeed * Time.deltaTime * _moveDirection);
        }
    }

    public bool IsWalking()
    {
        return playerController.velocity.magnitude > 0;
    }
}