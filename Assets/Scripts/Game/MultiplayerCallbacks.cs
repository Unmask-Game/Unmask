using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerCallbacks : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("On Disconnected");
        this.LoadMainMenu();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("On Master Client Switched");
        this.LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
