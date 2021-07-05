// Source: https://www.youtube.com/watch?v=_Zrde_WTaiI
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : MonoBehaviour
{
    private XRRig _xrRig;
    private CharacterController _characterController;
    private CharacterControllerDriver _driver;
    void Start()
    {
        _xrRig = GetComponent<XRRig>();
        _characterController = GetComponent<CharacterController>();
        _driver = GetComponent<CharacterControllerDriver>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCharacterController();
    }
    
    /// <summary>
    /// Update the <see cref="CharacterController.height"/> and <see cref="CharacterController.center"/>
    /// based on the camera's position.
    /// </summary>
    protected virtual void UpdateCharacterController()
    {
        if (_xrRig == null || _characterController == null)
            return;

        var height = Mathf.Clamp(_xrRig.cameraInRigSpaceHeight, _driver.minHeight, _driver.maxHeight);

        Vector3 center = _xrRig.cameraInRigSpacePos;
        center.y = height / 2f + _characterController.skinWidth;

        _characterController.height = height;
        _characterController.center = center;
    }
}
