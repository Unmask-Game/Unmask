using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_InputField roomNameText;

    public void JoinRoom()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Authenticating)
        {
            PhotonNetwork.NickName = SettingsManager.Instance.GetUsername();
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joining room");
        PhotonNetwork.JoinRoom(roomNameText.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Application Quit.");
        Application.Quit();
    }
}
