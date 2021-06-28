using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotonPlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject HUD;
    private PhotonView _view;
    
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        if (!_view.IsMine)
        {
            Camera.SetActive(false);
            HUD.SetActive(false);
            GetComponent<PlayerInput>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
