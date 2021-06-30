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

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 1F);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public float GetMouseSensitivity()
    {
        return PlayerPrefs.GetFloat("Mouse_Sensitivity", 1F);
    }

    public void SetMouseSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Mouse_Sensitivity", value);
    }
}
