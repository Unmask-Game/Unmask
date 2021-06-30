using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private TMP_InputField roomNameField;

    [SerializeField]
    private Button joinRoomButton;

    public void Start()
    {
        // Replace all letters with their uppercase variant
        roomNameField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
    }

    public void OnRoomNameFieldValueChange(string value)
    {
        // Only enable button if some room code was put in the text field
        joinRoomButton.interactable = !string.IsNullOrWhiteSpace(value);
    }

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
        PhotonNetwork.JoinRoom(roomNameField.text);
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
