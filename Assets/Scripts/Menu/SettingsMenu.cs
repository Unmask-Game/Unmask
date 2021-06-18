using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameText;
    
    public void OnEnable()
    {
        usernameText.text = SettingsManager.Instance.GetUsername();
    }

    public void SetUsername()
    {
        SettingsManager.Instance.SetUsername(usernameText.text);
    }
}
