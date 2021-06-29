using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;
public class VRPlayerController : MonoBehaviour
{
    public int resistancePoints;
    private CharacterController _controller;
    private PhotonView _view;

    // Start is called before the first frame update
    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        resistancePoints = 100;
    }
    
    private void Start()
    {
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            _controller = GameObject.FindGameObjectWithTag("VRRig").GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        if (VRManager.Instance.isVR && _view.IsMine)
        {
            transform.position = _controller.center;
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
}