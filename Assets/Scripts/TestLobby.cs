using System.Collections;
using System.Collections.Generic;
using ParrelSync;
using Photon.Pun;
using UnityEngine;

public class TestLobby : MonoBehaviourPunCallbacks
{
    
    [SerializeField] private bool preferVR;
    void Start()
    {
        PhotonNetwork.NickName = Random.Range(0, 100).ToString();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room was created");
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateLobby()
    {
        Debug.Log("Creating Room");
        PhotonNetwork.CreateRoom("test3");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinLobby()
    {
        Debug.Log("Joining Room");
        PhotonNetwork.JoinRoom("test3");
    }

    public void StartLobby()
    {
        if (ClonesManager.GetArgument().Equals("vr"))
        {
            VRManager.Instance.StartXR();
            VRManager.Instance.isVR = true;
        }
        PhotonNetwork.LoadLevel(5);
    }
}