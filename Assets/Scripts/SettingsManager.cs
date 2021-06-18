using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;
    public static SettingsManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
            gameObject.SetActive(false);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
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
