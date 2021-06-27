using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayerManager : MonoBehaviour
{

    [SerializeField] private GameObject XR;
    private PhotonView _view;
    
    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            XR.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
