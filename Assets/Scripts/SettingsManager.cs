using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    
    public String Username;
    void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            LoadPrefs();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadPrefs()
    {
        string defaultUsername = "Player" + Random.Range(1, 100);
        Username = PlayerPrefs.GetString("Username", defaultUsername);
    }

    public void SetUsername(string username)
    {
        Username = username;
        PlayerPrefs.SetString("Username", username);
    }
}
