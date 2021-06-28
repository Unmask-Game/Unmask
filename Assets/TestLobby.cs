using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TestLobby : MonoBehaviourPunCallbacks
{
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

    public void CreateLobby()
    {
        PhotonNetwork.CreateRoom("test");
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinRoom("test");
    }
    
    public void StartLobby()
    {
        PhotonNetwork.LoadLevel(5);
    }
}