using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: VR-player support
// TODO: Should be able to get a temporary speed boost or temporary slow down
public class PoliceMovement : MonoBehaviour
{
    [SerializeField] private CharacterController playerController;
    private float _originalSpeed;
    public float playerSpeed = 5;
    private float _slowdownExpiry;

    // Bottom of the player object
    [SerializeField] private Transform groundCheck;

    // Map/Environment the player is walking on
    [SerializeField] private LayerMask groundMask;

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
        _originalSpeed = playerSpeed;
        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //_items = GetComponent<ItemController>();
    }

    private void Update()
    {
        if (_view.IsMine)
        {
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

            // Gravity
            _verticalVelocity += Gravity * Time.deltaTime;
            playerController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);

            if (Time.time > _slowdownExpiry)
            {
                playerSpeed = _originalSpeed;
            }

            playerController.Move(playerSpeed * Time.deltaTime * (transform.rotation * _moveDirection));
        }
    }

    // Can be used for freezing police players at the start of a round for 10 secs e.g.
    public void SetTemporarySpeed(float speed, float duration)
    {
        playerSpeed = speed;
        _slowdownExpiry = duration + Time.time;
    }
}