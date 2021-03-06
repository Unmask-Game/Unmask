using System;
using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using static DefaultNamespace.Constants;

public class VRPlayerController : MonoBehaviour
{
    [SerializeField] private GameObject rope;
    [SerializeField] private ParticleSystem electricity;
    [SerializeField] private ParticleSystem exhausted;
    [SerializeField] private AudioManager audioManager;
    private AudioSource _batonHitSound;
    private AudioSource _taserHitSound;
    public int resistancePoints;
    public bool isArrestable;
    private CharacterController _controller;
    private GameObject _xrRig;
    private PhotonView _view;
    private GameObject _camera;
    private Animator _animator;
    private ActionBasedContinuousMoveProvider _moveProvider;
    private int _slowDownTicks;

    private void Awake()
    {
        rope.SetActive(false);
        resistancePoints = Constants.ThiefResistancePoints;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _view = GetComponent<PhotonView>();
        // If is VRPlayer and spawned by me
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            // Set XRRig components
            _xrRig = GameObject.FindGameObjectWithTag("XRRig");
            _controller = _xrRig.GetComponent<CharacterController>();
            _camera = _xrRig.transform.Find("Camera Offset").Find("Main Camera").gameObject;
            _moveProvider = _xrRig.transform.Find("Locomotion System")
                .GetComponent<ActionBasedContinuousMoveProvider>();
            _moveProvider.moveSpeed = VRWalkSpeed;

            // Disable VRPlayer prefab visuals locally (if VRPlayer)
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            // Disable player input if not mine
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    private void Update()
    {
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            // teleport VRPlayer prefab to VR CharacterController position
            transform.position = _xrRig.transform.position + _xrRig.transform.rotation * _controller.center +
                                 new Vector3(0, -(_controller.height / 2), 0);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                _camera.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }

    public void FixedUpdate()
    {
        if (_view.IsMine)
        {
            // Change speed of VRPlayer if slowed down or sprinting
            var inputDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            float speed;
            if (--_slowDownTicks > 0)
            {
                speed = VRSlowedSpeed;
            }
            else
            {
                inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out bool sprinting);
                speed = sprinting ? VRSprintSpeed : VRWalkSpeed;
            }

            _moveProvider.moveSpeed = isArrestable ? speed * SpeedMultiplier : speed;

            // Check if player is walking
            inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 move);
            bool walking = move.sqrMagnitude > 0.1;

            if (_animator.GetBool("walking") != walking)
            {
                SetIsWalking(walking);
                _view.RPC("SetIsWalking", RpcTarget.Others, walking);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, Item.ItemName causedBy)
    {
        // Send damage to all connected peers
        _view.RPC("TakeDamageRemote", RpcTarget.All, damage, causedBy);
    }

    [PunRPC]
    public void TakeDamageRemote(int damage, Item.ItemName causedBy)
    {
        resistancePoints -= damage;
        if (resistancePoints <= 0)
        {
            isArrestable = true;
            exhausted.Play();
        }

        if (causedBy == Item.ItemName.Baton)
        {
            OnBatonHit();
        }
        else if (causedBy == Item.ItemName.Taser)
        {
            OnTaserHit();
        }
    }

    private void OnBatonHit()
    {
        if (!_batonHitSound)
        {
            _batonHitSound = audioManager.GetSound("Oof");
        }

        _batonHitSound.Play();
    }

    private void OnTaserHit()
    {
        if (!_taserHitSound)
        {
            _taserHitSound = audioManager.GetSound("Buzz");
        }

        _taserHitSound.Play();
        electricity.Play();
    }

    // called when player is hit by handcuffs
    [PunRPC]
    public void BeArrested()
    {
        if (_view.IsMine)
        {
            // If HP <= 0
            if (!isArrestable) return;
            _view.RPC("BeArrestedRemote", RpcTarget.All);
        }
        else
        {
            _view.RPC("BeArrested", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void BeArrestedRemote()
    {
        GameStateManager.Instance.EndGame(false);
    }

    // slow down player if hit by lasso
    public void SlowDown(float seconds)
    {
        int ticks = (int)Math.Ceiling(seconds * 50);
        if (ticks > _slowDownTicks)
        {
            _slowDownTicks = ticks;
        }
    }
    
    public void OnLassoHit(float seconds)
    {
        _view.RPC("OnLassoHitRemote", RpcTarget.All, seconds);
    }

    [PunRPC]
    public void OnLassoHitRemote(float seconds)
    {
        StartCoroutine(DisplayRope(seconds));
        if (_view.IsMine)
        {
            SlowDown(seconds);
        }
    }

    // Display rope line between players for specific time
    private IEnumerator DisplayRope(float time)
    {
        rope.SetActive(true);
        yield return new WaitForSeconds(time);
        rope.SetActive(false);
    }

    // Sync walking for animation
    [PunRPC]
    public void SetIsWalking(Boolean walking)
    {
        _animator.SetBool("walking", walking);
    }

    public float GetResistancePointsPercentile()
    {
        return (float)resistancePoints / Constants.ThiefResistancePoints;
    }
}