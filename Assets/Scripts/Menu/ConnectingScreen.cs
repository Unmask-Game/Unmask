using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectingScreen : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private MainMenu _mainMenu;

    [SerializeField]
    private Button _okButton;

    [SerializeField]
    private TMP_Text _connectingText;

    public override void OnConnectedToMaster()
    {
        Debug.Log("Joining room");
        PhotonNetwork.JoinRoom(_mainMenu.roomNameField.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room");
        this.ResetScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed: " + message);
        // Show ok button and fail reson
        _connectingText.text = message;
        _okButton.gameObject.SetActive(true);
        PhotonNetwork.Disconnect();
    }

    public void PressOK()
    {
        // Switch back to main menu
        this.ResetScene();
        gameObject.SetActive(false);
        _mainMenu.gameObject.SetActive(true);
    }

    private void ResetScene()
    {
        _connectingText.text = "Connecting...";
        _okButton.gameObject.SetActive(false);
    }
}
