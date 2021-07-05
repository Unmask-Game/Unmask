using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerCallbacks : MonoBehaviourPunCallbacks
{
    public override void OnDisconnected(DisconnectCause cause)
    {
        this.LoadMainMenu("Connection to server lost.");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        this.LoadMainMenu("The VR Player disconnected.");
    }

    private void LoadMainMenu(string reason)
    {
        PhotonNetwork.Disconnect();

        // We do not want to override the initial disconnect reason, if there are multiple
        if (ConnectingScreen.DisconnectReason == null)
        {
            ConnectingScreen.DisconnectReason = reason;
        }

        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
