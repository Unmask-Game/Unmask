using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomMenu : MonoBehaviour
{
    
    [SerializeField]
    private TMP_Text roomCodeText;
    void Start()
    {
        roomCodeText.text = PhotonNetwork.CurrentRoom.Name;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(player.Value.NickName);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
