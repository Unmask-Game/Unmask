using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomCodeText;
    private PhotonView _view;

    private GameObject[] playerNames;
    void Start()
    {
        // Set timescale back to one for consecutive games (timescale is set to 0 after the game is over)
        Time.timeScale = 1;
        _view = GetComponent<PhotonView>();
        roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        playerNames = GameObject.FindGameObjectsWithTag("PlayerName");
        DrawPlayers();
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    [PunRPC]
    public void StartRemote()
    {
        GameStateManager.Instance.StartGame();
        PhotonNetwork.LoadLevel(5);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined!");
        DrawPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DrawPlayers();
    }

    public void StartGame()
    {
        _view.RPC("StartRemote", RpcTarget.All);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // When the original Master Client disconnects, the room should be closed.
        Debug.Log("Master disconnected");
        ConnectingScreen.DisconnectReason = "The room was closed.";
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Menu");
    }

    public void QuitRoom()
    {
        Debug.Log("Quit Room");
        PhotonNetwork.Disconnect();

        // If the Room is left, show back menu screen
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master quit");
            PhotonNetwork.LoadLevel("VRMenu");
        }
        else
        {
            Debug.Log("slave quit");
            PhotonNetwork.LoadLevel("Menu");
        }
    }

    void DrawPlayers()
    {
        int index = 0;
        // Put playernames in the text boxes
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            playerNames[index].GetComponent<TMP_Text>().text = player.Value.NickName;
            index++;
        }

        // Clear other text boxes
        for (int i = index; i < playerNames.Length; i++)
        {
            playerNames[index].GetComponent<TMP_Text>().text = "";
        }
    }
}
