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
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinRoom("test3");
    }

    public void StartLobby()
    {
        PhotonNetwork.LoadLevel(5);
    }
}