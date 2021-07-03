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
    [SerializeField] private Button startButton;
    private PhotonView _view;

    private GameObject[] playerNames;
    void Start()
    {
        Time.timeScale = 1;
        _view = GetComponent<PhotonView>();
        roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        playerNames = GameObject.FindGameObjectsWithTag("PlayerName");
        drawPlayers();
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
        drawPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        drawPlayers();
    }

    public void StartGame()
    {
        _view.RPC("StartRemote",RpcTarget.All);
    }

    void drawPlayers()
    {
        int index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            playerNames[index].GetComponent<TMP_Text>().text = player.Value.NickName;
            index++;
        }
    }
}
