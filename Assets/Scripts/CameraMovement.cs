using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Optional settings
    [SerializeField] private float sensitivity = 400f;
    [SerializeField] private bool smoothing;

    // X-axis Camera rotation
    private float _xRot;

    // Mouse Input
    private float _mouseX;
    private float _mouseY;
    private bool _cursorIsLocked;

    private void Awake()
    {
        // Hiding Mouse Cursor
        _cursorIsLocked = true;
        //Cursor.lockState = CursorLockMode.Locked;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        // Getting mouse input and adjusting it with delta times 
        _mouseX = Input.GetAxis("Mouse X") * sensitivity * (smoothing == false ? Time.deltaTime : Time.smoothDeltaTime);
        _mouseY = Input.GetAxis("Mouse Y") * sensitivity * (smoothing == false ? Time.deltaTime : Time.smoothDeltaTime);

        _xRot -= _mouseY;
        _xRot = Mathf.Clamp(_xRot, -60, 60);

        transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
        player.Rotate(Vector3.up * _mouseX);
        
        // Unlocking cursor, if settings are opened
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cursorIsLocked = !_cursorIsLocked;
        }
        Cursor.lockState = _cursorIsLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}