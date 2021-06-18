using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    
    const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    public void CreateRoom()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.ConnectingToMasterServer)
        {
            PhotonNetwork.NickName = SettingsManager.Instance.GetUsername();
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.CreateRoom(generateRoomName(), new RoomOptions() { MaxPlayers = 4 });
    }

    public override void OnCreatedRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private string generateRoomName()
    {
        string roomName = "";
        for (int i = 0; i < 5; i++)
        {
            roomName += chars[Random.Range(0, chars.Length)];
        }

        return roomName;
    }
}
