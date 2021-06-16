using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    private string roomName;
    private bool creating = false;

    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnConnectedToMaster()
    {
        if (creating)
        {
            Debug.Log("Connected to master server");
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 4 });
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }

    }

    public void CreateRoom(string roomName)
    {
        this.roomName = roomName;
        creating = true;
        PhotonNetwork.NickName = SettingsManager.instance.Username;
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnCreatedRoom()
    {
        if (creating)
        {
            Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
            ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName);
    }

    public void JoinRoom(string roomName)
    {
        this.roomName = roomName;
        creating = false;
        PhotonNetwork.NickName = SettingsManager.instance.Username;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ChangeScene(int sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
}
