using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager
{
    private static SettingsManager _instance;
    public static SettingsManager Instance { get { return _instance; } }

    static SettingsManager()
    {
        _instance = new SettingsManager();
    }

    public void SetUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
    }

    public string GetUsername()
    {
        string defaultUsername = "Player" + Random.Range(1, 100);
        return PlayerPrefs.GetString("Username", defaultUsername);
    }
}
