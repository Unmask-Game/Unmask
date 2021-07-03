using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNameField;

    [SerializeField]
    private Button joinRoomButton;

    [SerializeField]
    private ConnectingScreen _connectScreen;

    public void Start()
    {
        // Replace all letters with their uppercase variant
        roomNameField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };

        // Was the menu scene loaded unexcpectedly? (eg. on Disconnect)
        if (ConnectingScreen.DisconnectReason != null)
        {
            ShowConnectScreen();
        }
    }

    public void Update()
    {
        // When RETURN is pressed, join the room
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            this.JoinRoom();
        }
    }

    public void OnRoomNameFieldValueChange(string value)
    {
        // Only enable button if some room code was put in the text field
        joinRoomButton.interactable = !string.IsNullOrWhiteSpace(value);
    }

    public void JoinRoom()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
        if (!IsConnected())
        {
            ShowConnectScreen();
            PhotonNetwork.NickName = SettingsManager.Instance.GetUsername();
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void ShowConnectScreen()
    {
        gameObject.SetActive(false);
        _connectScreen.gameObject.SetActive(true);
    }

    private bool IsConnected()
    {
        return PhotonNetwork.NetworkClientState != ClientState.Disconnected && PhotonNetwork.NetworkClientState != ClientState.PeerCreated;
    }

    public void Quit()
    {
        Debug.Log("Application Quit.");
        Application.Quit();
    }
}
