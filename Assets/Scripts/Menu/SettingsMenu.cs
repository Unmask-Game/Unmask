using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameText;

    public void OnEnable()
    {
        usernameText.text = SettingsManager.instance.Username;
    }

    public void SetUsername()
    {
        SettingsManager.instance.SetUsername(usernameText.text);
    }
}
