using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomCodeText;

    private GameObject[] playerNames;
    void Start()
    {
        roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        playerNames = GameObject.FindGameObjectsWithTag("PlayerName");
        DrawPlayers();
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

    void DrawPlayers()
    {
        int index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            playerNames[player.Key - 1].GetComponent<TMP_Text>().text = player.Value.NickName;
            index++;
        }
    }
}
