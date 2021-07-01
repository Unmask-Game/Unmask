using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static DefaultNamespace.Constants;

public class VRPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rope;
    public int resistancePoints;
    private CharacterController _controller;
    private GameObject _xrRig;
    private PhotonView _view;
    private GameObject _camera;
    private Animator _animator;
    private ActionBasedContinuousMoveProvider _moveProvider;

    // Start is called before the first frame update
    private void Awake()
    {
        rope.SetActive(false);
        resistancePoints = 100;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            _xrRig = GameObject.FindGameObjectWithTag("XRRig");
            _controller = _xrRig.GetComponent<CharacterController>();
            _camera = _xrRig.transform.Find("Camera Offset").Find("Main Camera").gameObject;
            _moveProvider = _xrRig.transform.Find("Locomotion System")
                .GetComponent<ActionBasedContinuousMoveProvider>();
            _moveProvider.moveSpeed = VRWalkSpeed;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            // Disable player input if not is mine
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    private void Update()
    {
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            transform.position = _xrRig.transform.position + _xrRig.transform.rotation * _controller.center + new Vector3(0, -(_controller.height / 2), 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    public void TakeDamage(int damage)
    {
        resistancePoints -= damage;
        Debug.Log("Damn, I got hit for -" + damage + " .... Current RP: " + resistancePoints);
    }

    public void BeArrested()
    {
        if (resistancePoints > 0) return;
        Debug.Log("Damn, I've been arrested");
        Destroy(gameObject);
    }

    // called when player is hit by lasso
    public void BeSlowedDown(float time)
    {
        StartCoroutine(DisplayRope(time));
        Debug.Log("Damn, I've been slowed down");
    }

    private IEnumerator DisplayRope(float time)
    {
        rope.SetActive(true);
        yield return new WaitForSeconds(time);
        rope.SetActive(false);
    }

    public void FixedUpdate()
    {
        if (_view.IsMine)
        {
            var inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out bool sprinting);
            _moveProvider.moveSpeed = sprinting ? VRSprintSpeed : VRWalkSpeed;

            inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 move);
            bool walking = move.sqrMagnitude > 0.1;
            
            if (_animator.GetBool("walking") != walking)
            {
                SetIsWalking(walking);
                Debug.Log("Set walking to: " + walking);
                _view.RPC("SetIsWalking", RpcTarget.Others, walking);
            }
        }
    }

    [PunRPC]
    public void SetIsWalking(Boolean walking)
    {
        _animator.SetBool("walking", walking);
    }
}