using UnityEngine;

// TODO: VR-player support
// TODO: Add Animation Controller
// TODO: Should be able to get a temporary speed boost or temporary slow down
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    public float playerSpeed = 5;

    private Vector3 _moveDirection;

    // Bottom of the player object
    [SerializeField] private Transform groundCheck;

    // Map/Environment the player is walking on
    [SerializeField] private LayerMask groundMask;
    
    private const float GroundDistance = 0.4f;
    private const float Gravity = -8;
    private float _verticalVelocity;

    // Keyboard/Controller Input used as Vertical and Horizontal Movement
    private float _moveX;
    private float _moveZ;

    private void Update()
    {
        GetKeyboardInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void GetKeyboardInput()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveZ = Input.GetAxis("Vertical");

        var self = transform;
        _moveDirection = self.right * _moveX + self.forward * _moveZ;
    }

    private void Movement()
    {
        // No Vertical Velocity (Gravity) if the player is already touching the ground
        if (Physics.CheckSphere(groundCheck.position, GroundDistance, groundMask) && _verticalVelocity < 0)
        {
            _verticalVelocity = 0;
        }
        else
        {
            _verticalVelocity += Gravity * Time.deltaTime;
            playerController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
        }

        playerController.Move(playerSpeed * Time.deltaTime * _moveDirection);
    }
}