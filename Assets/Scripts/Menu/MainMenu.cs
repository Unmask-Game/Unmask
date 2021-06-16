using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; //add the characters you want

    [SerializeField] private TMP_InputField roomCodeText;

    public async void CreateRoom()
    {
        await NetworkManager.instance.CreateRoom(generateRoomName());
    }

    public void JoinRoom()
    {
        NetworkManager.instance.JoinRoom(roomCodeText.text);
    }

    private string generateRoomName()
    {
        string roomName = "";
        for (int i = 0; i < 5; i++)
        {
            roomName += chars[Random.Range(0, chars.Length)];
        }

        return roomName;
    }
}