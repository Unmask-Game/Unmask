using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class VRPlayerController : MonoBehaviour
{
    public int resistancePoints;
    private CharacterController _controller;
    private GameObject _xrRig;
    private PhotonView _view;
    private GameObject _camera;
    private Animator _animator;
    private int _walking;

    // Start is called before the first frame update
    private void Awake()
    {

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
        Debug.Log("Damn, I've been slowed down");
    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        _walking = 5;
    }

    public void FixedUpdate()
    {
        bool walking = _walking > 0;
        if (_animator.GetBool("walking") != walking)
        {
            SetIsWalking(walking);
            Debug.Log("Setting is walking: " + walking);
            _view.RPC("SetIsWalking", RpcTarget.Others, walking);
        }
        if (walking)
        {
            _walking--;
        }
    }

    [PunRPC]
    public void SetIsWalking(Boolean walking)
    {
        _animator.SetBool("walking", walking);
    }
}