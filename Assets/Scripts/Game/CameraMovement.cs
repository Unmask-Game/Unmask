using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Optional settings
    [SerializeField] private float sensitivity;
    [SerializeField] private bool smoothing;

    // X-axis Camera rotation
    private float _xRot;

    // Mouse Input
    private float _mouseX;
    private float _mouseY;
    private bool _cursorIsLocked;

    private void Awake()
    {
        // Hiding Mouse Cursor for non-VR players
        if (!PhotonNetwork.IsMasterClient)
        {
            _cursorIsLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.sensitivity = SettingsManager.Instance.GetMouseSensitivity();
    }

    public void Look(InputAction.CallbackContext context)
    {
        // Getting mouse input and adjusting it with delta times 
        var input = context.ReadValue<Vector2>();
        _mouseX = input.x * sensitivity * 25;
        _mouseY = input.y * sensitivity * 15;

    }

    public void LockCursor(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            _cursorIsLocked = !_cursorIsLocked;
            Cursor.lockState = _cursorIsLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        _xRot -= _mouseY * (smoothing == false ? Time.deltaTime : Time.smoothDeltaTime);
        _xRot = Mathf.Clamp(_xRot, -60, 60);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = _xRot;
        transform.eulerAngles = targetRotation;

        //transform.localRotation = Quaternion.Euler(_xRot, 0, 0);
        player.Rotate(Vector3.up, _mouseX * (smoothing == false ? Time.deltaTime : Time.smoothDeltaTime));
    }
}